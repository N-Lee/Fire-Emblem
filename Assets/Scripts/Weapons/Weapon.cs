using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {sword, axe, lance, staff, wind, fire, thunder, light, dark, anima, empty};
public class Weapon
{
    public int minRange, maxRange, durabilty, weight, might, hitRate, critRate, weaponExp, cost;
    public char rank;
    public string name;
    public bool isPhysical;
    public WeaponType weaponType;
    public string imageLocation;

    public int Advantage(WeaponType enemy)
    {
        switch (weaponType)
        {
            case WeaponType.sword:
                if (enemy == WeaponType.axe)
                {
                    return 1;
                }
                else if (enemy == WeaponType.lance)
                {
                    return -1;
                }
                
                break;

            case WeaponType.axe:
                if (enemy == WeaponType.lance)
                {
                    return 1;
                }
                else if (enemy == WeaponType.sword)
                {
                    return -1;
                }

                break;

            case WeaponType.lance:
                if (enemy == WeaponType.sword)
                {
                    return 1;
                }
                else if (enemy == WeaponType.axe)
                {
                    return -1;
                }

                break;

            case WeaponType.wind:
            case WeaponType.fire:
            case WeaponType.thunder:
                if (enemy == WeaponType.light)
                {
                    return 1;
                }
                else if (enemy == WeaponType.dark)
                {
                    return -1;
                }

                break;

            case WeaponType.light:
                if (enemy == WeaponType.dark)
                {
                    return 1;
                }
                else if (enemy == WeaponType.wind  || enemy == WeaponType.fire || enemy == WeaponType.thunder)
                {
                    return -1;
                }

                break;

            case WeaponType.dark:
                if (enemy == WeaponType.wind  || enemy == WeaponType.fire || enemy == WeaponType.thunder)
                {
                    return 1;
                }
                else if (enemy == WeaponType.light)
                {
                    return -1;
                }

                break;
        }

        return 0;
    }
}
