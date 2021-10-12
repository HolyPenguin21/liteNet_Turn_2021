using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiSpearman : PlayerItem
{
    public PiSpearman()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 6;
        this.name = "Spearman";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
        this.oneTime = true;
    }

    public override void Effect()
    {
        Account account = GameData.inst.account;
        CharactersData cd = new CharactersData();
        LocalData ld = new LocalData();

        Character character = cd.Get_Character_ById(2);

        account.сharacters.Add(character);

        ld.Save_PlayerData(account);
    }
}
