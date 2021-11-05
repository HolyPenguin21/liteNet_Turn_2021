using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

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

        AttackData aData = JsonUtility.FromJson<AttackData>(attackData);
        for (int x = 0; x < aData.attacks.Count; x++)
        {
            AttackHit aHit = aData.attacks[x];

            if(aHit.role == "a")
            {
                yield return attacker.Attack_Animation();
                yield return target.Set_Health(aHit.t_Health);
                DmgEffect(aHit.dmg, target.hex, true);

                if (GameData.inst.server != null)
                    if (aHit.t_Health <= 0)
                    {
                        GameData.inst.gameMain.Order_Die(target.hex);
                    }
            }
            else
            {
                yield return target.Attack_Animation();
                yield return attacker.Set_Health(aHit.t_Health);
                DmgEffect(aHit.dmg, attacker.hex, false);

                if (GameData.inst.server != null)
                    if (aHit.t_Health <= 0)
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
        AttackData aData = new AttackData();

        int a_Health = attacker.health.hp_cur;
        int a_AttackCount = attacker.attacks[this.a_AttackId].attacksCount_cur;

        int t_Health = target.health.hp_cur;
        int t_AttackCount = 0;
        if (this.t_AttackId != -1) t_AttackCount = target.attacks[this.t_AttackId].attacksCount_cur;

        while (a_AttackCount > 0 || t_AttackCount > 0)
        {
            if (a_AttackCount > 0)
            {
                CharAttack a_Attack = attacker.attacks[this.a_AttackId];

                int dmg = AttackResult_Calculation(a_Attack, target);
                t_Health = Target_HealthCalculation(dmg, t_Health);

                AttackHit aHit = new AttackHit("a", dmg, t_Health);
                aData.attacks.Add(aHit);

                if (t_Health <= 0) break;

                a_AttackCount--;
            }

            if (t_AttackCount > 0)
            {
                CharAttack t_Attack = target.attacks[this.t_AttackId];

                int dmg = AttackResult_Calculation(t_Attack, attacker);
                a_Health = Target_HealthCalculation(dmg, a_Health);

                AttackHit tHit = new AttackHit("t", dmg, a_Health);
                aData.attacks.Add(tHit);

                if (a_Health <= 0) break;

                t_AttackCount--;
            }
        }

        return JsonUtility.ToJson(aData);
    }

    private int AttackResult_Calculation(CharAttack a_Attack, Character target)
    {
        AttackCalculation attackCalculation = new AttackCalculation();

        int dodge = target.defence.dodgeChance + target.hex.dodge;
        if (UnityEngine.Random.Range(0, 101) < dodge)
        {
            return -1;
        }
        else 
        {
            return attackCalculation.Hit_DmgCalculation(a_Attack, target);
        }
    }

    private int AttackTrauma_Calculation(int dmg, CharAttack a_Attack, Character target, string result)
    {
        if(dmg <= 0) return -1;

        AttackCalculation attackCalculation = new AttackCalculation();

        int traumaChance = 100;

        if (UnityEngine.Random.Range(0, 101) < traumaChance)
        {
            int tempId = attackCalculation.Hit_Trauma(a_Attack, target);
            // if(DuplicatedTrauma())
            // {
            //     return -1;
            // }
            // else
            // {
                return tempId;
            // }
        }
        else
        {
            return -1;
        }
    }

    private int Target_HealthCalculation(int a_dmg, int t_Health)
    {
        if(a_dmg <= 0) return t_Health;
        return t_Health - a_dmg;
    }

    private void DmgEffect(int dmg, Hex hex, bool isAttacker)
    {
        if(dmg >= 0)
        {
            if(isAttacker)
                Effects.Dmg_Red(hex, dmg);
            else
                Effects.Dmg_Blue(hex, dmg);
        }
        else
        {
            Effects.Miss(hex, dmg);
        }
    }

    [System.Serializable]
    public class AttackData
    {
        public List<AttackHit> attacks = new List<AttackHit>();
    }

    [System.Serializable]
    public class AttackHit
    {
        public string role;
        public int dmg;
        public int t_Health;

        public AttackHit(string role, int dmg, int t_Health)
        {
            this.role = role;
            this.dmg = dmg;
            this.t_Health = t_Health;
        }
    }
}
