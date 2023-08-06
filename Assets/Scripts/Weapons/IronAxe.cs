using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronAxe : Weapon
{
    public IronAxe()
    {
        minRange = 1;
        maxRange = 1;
        durabilty = 45;
        weight = 10;
        might = 6;
        hitRate = 65;
        critRate = 0;
        weaponExp = 1;
        cost = 270;
        rank = 'E';
        name = "Iron Axe";
        weaponType = WeaponType.axe;
        isPhysical = true;
        imageLocation = "Image/Weapons/IronAxe";
    }
}
