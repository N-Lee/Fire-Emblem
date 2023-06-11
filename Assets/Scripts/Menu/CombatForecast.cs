using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatForecast : Menu
{
    Unit player, enemy;
    [SerializeField] TMP_Text pName, pHp, pMt, pHit, pCrit, eName, eWeaponName, eHp, eMt, eHit, eCrit;
    [SerializeField] Image pWeaponIcon, pAdvantage, eWeaponIcon, eAdvantage;

    public void GetUnits(Unit player, Unit enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

    public override void Show(Vector3 unitPosition)
    {
        gameObject.SetActive(true);
        MoveMenu(unitPosition);
        SetUnitValues();
    }

    void SetUnitValues()
    {
        SetNames();
        SetWeapon();
        SetWeaponAdvantages();
        SetHp();
        SetMt();
        SetHit();
        SetCrit();
    }

    void SetNames()
    {
        pName.text = player.name;
        eName.text = enemy.name;
    }

    void SetWeapon()
    {
        Sprite icon = Resources.Load <Sprite>(player.equippedWeapon.imageLocation);
        pWeaponIcon.sprite = icon;

        icon = Resources.Load <Sprite>(enemy.equippedWeapon.imageLocation);
        eWeaponIcon.sprite = icon;

        eWeaponName.text = enemy.equippedWeapon.name;
    }

    void SetWeaponAdvantages()
    {
        int advantage = player.equippedWeapon.Advantage(enemy.equippedWeapon.weaponType);

        if (advantage == 1)
        {
            pAdvantage.color = new Color32(0, 255, 0, 100);
            eAdvantage.color = new Color32(255, 0, 0, 100);
        }
        else if (advantage == -1)
        {
            pAdvantage.color = new Color32(0, 255, 0, 100);
            eAdvantage.color = new Color32(255, 0, 0, 100);
        }
        else if (advantage == 0)
        {
            pAdvantage.color = new Color32(0, 255, 0, 0);
            eAdvantage.color = new Color32(255, 0, 0, 0);
        }
    }

    void SetHp()
    {
        pHp.text = player.currentHp.ToString();
        eHp.text = enemy.currentHp.ToString();
    }

    void SetMt()
    {
        pMt.text = player.might.ToString();
        eMt.text = enemy.might.ToString();
    }

    void SetHit()
    {
        pHit.text = ((int)player.DisplayedHitRate()).ToString();
        eHit.text = ((int)enemy.DisplayedHitRate()).ToString();
    }

    void SetCrit()
    {
        pCrit.text = ((int)player.CritRate()).ToString();
        eCrit.text = ((int)enemy.CritRate()).ToString();
    }
}
