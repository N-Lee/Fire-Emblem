using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWeapon : Weapon
{
    public NoWeapon()
    {
        minRange = 0;
        maxRange = 0;
        durabilty = 0;
        weight = 0;
        might = 0;
        hitRate = 0;
        critRate = 0;
        weaponExp = 0;
        cost = 0;
        rank = 'E';
        name = "No Weapon";
        weaponType = WeaponType.empty;
        isPhysical = true; 
    }
}
