using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSword : Weapon
{
    public IronSword() 
    {
        minRange = 1;
        maxRange = 1;
        durabilty = 46;
        weight = 5;
        might = 5;
        hitRate = 90;
        critRate = 0;
        weaponExp = 1;
        cost = 460;
        rank = 'E';
        name = "Iron Sword";
        weaponType = WeaponType.sword;
        isPhysical = true;
        imageLocation = "Image/Weapons/IronSword";
    }
}
