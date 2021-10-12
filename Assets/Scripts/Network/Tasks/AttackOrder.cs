using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class AttackOrder : GeneralNetworkTask
{
    public int attacker_x { get; set; }
    public int attacker_y { get; set; }
    public int a_AttackId { get; set; }

    public int target_x { get; set; }
    public int target_y { get; set; }
    public int t_AttackId { get; set; }

    public string attackData { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        yield return Implementation();
       
        TaskDone(taskId);
    }

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : Attck order, id : " + taskId);
        for (int x = 0; x < server.players.Count; x++)
        {
            Account player = server.players[x];
            if (player.isServer) continue;
            server.netProcessor.Send(player.address, this, DeliveryMethod.ReliableOrdered);
        }
    }

    public override IEnumerator Implementation_Client()
    {
        Client client = GameData.inst.client;
        if(client == null) yield return null;

        yield return Implementation();

        TaskDone(taskId);
    }

    public override void RequestServer()
    {
        
    }

    private IEnumerator Implementation()
    {
        Hex a_Hex = Utility.Get_Hex_ByCoords(attacker_x, attacker_y);
        Character attacker = a_Hex.character;

        Hex t_Hex = Utility.Get_Hex_ByCoords(target_x, target_y);
        Character target = t_Hex.character;

        // move In
        yield return attacker.Attack_Start(t_Hex);
        
        if(target == null) yield break;
            yield return target.Attack_Start(a_Hex);

        string[] data = attackData.Split(';');
        for (int x = 0; x < data.Length; x++)
        {
            string[] attackData = data[x].Split(',');
            string role = attackData[0]; // 'a' or 't'
            int dmg = Convert.ToInt32(attackData[1]);
            int healthLeft = Convert.ToInt32(attackData[2]);

            if(role == "a")
            {
                yield return attacker.Attack_Animation();
                yield return target.Set_Health(healthLeft);
                // Effect
                if(dmg >= 0) Effects.Dmg(target.hex, dmg, true);
                else Effects.Miss(target.hex, dmg);

                if (GameData.inst.server != null)
                    if (healthLeft <= 0)
                    {
                        GameData.inst.gameMain.Order_Die(target.hex);
                    }
            }
            else
            {
                yield return target.Attack_Animation();
                yield return attacker.Set_Health(healthLeft);
                // Effect
                if(dmg >= 0) Effects.Dmg(attacker.hex, dmg, false);
                else Effects.Miss(attacker.hex, dmg);

                if (GameData.inst.server != null)
                    if (healthLeft <= 0)
                    {
                        GameData.inst.gameMain.Order_Die(attacker.hex);
                    }
            }
        }
        // move Out
        if (attacker.health.hp_cur > 0) 
        {
            yield return attacker.Attack_End();
            if(attacker.owner.aiPlayer)
                GameData.inst.gameMain.sceneMain.aiBehaviour.aiInAction = false;
        }
        else 
        {
            if(attacker.owner.aiPlayer) 
                GameData.inst.gameMain.sceneMain.aiBehaviour.aiInAction = false;
        }
        if (target.health.hp_cur > 0) yield return target.Attack_End();
    }

    public string Get_AttackData(Character attacker, Character target)
    {
        int a_Health = attacker.health.hp_cur;
        int a_AttackCount = attacker.attacks[this.a_AttackId].attacksCount;

        int t_Health = target.health.hp_cur;
        int t_AttackCount = 0;
        if (this.t_AttackId != -1) t_AttackCount = target.attacks[this.t_AttackId].attacksCount;

        string result = "";

        while (a_AttackCount > 0 || t_AttackCount > 0)
        {
            if (a_AttackCount > 0)
            {
                CharAttack a_Attack = attacker.attacks[this.a_AttackId];
                int dmg = AttackResult_Calculation(a_Attack, target);
                t_Health = Target_HealthCalculation(dmg, t_Health);

                result += "a," + dmg + "," + t_Health + ";";

                if (t_Health <= 0) break;

                a_AttackCount--;
            }

            if (t_AttackCount > 0)
            {
                CharAttack t_Attack = target.attacks[this.t_AttackId];
                int dmg = AttackResult_Calculation(t_Attack, attacker);
                a_Health = Target_HealthCalculation(dmg, a_Health);

                result += "t," + dmg + "," + a_Health + ";";

                if (a_Health <= 0) break;

                t_AttackCount--;
            }
        }
        if(result != "") result = result.Remove(result.Length - 1);

        return result;
    }

    private int AttackResult_Calculation(CharAttack a_Attack, Character target)
    {
        AttackCalculation attackCalculation = new AttackCalculation();

        int dodge = target.defence.dodgeChance + target.hex.dodge;
        if (UnityEngine.Random.Range(0, 101) < dodge) return -1;
        else return attackCalculation.Hit_DmgCalculation(a_Attack, target);
    }

    private int Target_HealthCalculation(int a_dmg, int t_Health)
    {
        if (a_dmg <= 0) return t_Health;

        return t_Health -= a_dmg;
    }
}
