using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int level, exp, hp, strength, skill, speed, luck, defense, resistance, constitution, move;
    public UnitClass unitClass;
    public bool isBoss;
    IDictionary<string, int> weapons;

    // NOTES: Weapon EXP: Get 2 per attack (4 if the attacks twice in one turn). Start at 1 (E). 31 is D. 71 is C (+40). 121 is B. 181 is A. 251 is S 
    // Weapon rank bonus https://fireemblem.fandom.com/wiki/Weapon_Level

    public void SetClass(UnitClass newClass)
    {
        unitClass = newClass;
        bool emptyWeapons = weapons.Count == 0 ? true : false;

        for (int i = 0; i < unitClass.weapons.Count; i++)
        {
            string weaponName = unitClass.weapons[i];
            int baseWeaponLevel = unitClass.baseWeaponLevel[i];

            if (emptyWeapons)
            {
                weapons.Add(weaponName, baseWeaponLevel);
                continue;
            }

            for (int j = 0; j < weapons.Count; j++)
            {
                if (weapons.ContainsKey(weaponName))
                {
                    weapons[weaponName] = weapons[weaponName] > baseWeaponLevel ? weapons[weaponName]: baseWeaponLevel;
                }
                else 
                {
                    weapons.Add(weaponName, baseWeaponLevel);
                }
            }
        }
    }

    public int AttackDamage(int enemyDef, int enemyRes, int weaponTriangle)
    {
        int damage = 0;

        if (unitClass.isPhysical)
        {
            damage = strength - enemyDef;
        }
        else
        {
            damage = strength - enemyRes;
        }

        return damage + weaponTriangle;
    }

    public int AttackSpeed(int enemySpeed)
    {
        return speed - enemySpeed >= 5 ? 2 : 1;
    }

    public int CritRate(int weaponCrit)
    {
        return skill/2 + weaponCrit;
    }

    public int HitRate(int weaponHit)
    {
        return ((skill * 3 + luck) / 2) + weaponHit;
    }

    public int AvoidRate(int weaponTriangle)
    {
        return ((speed * 3 + luck) / 2) + weaponTriangle;
    }

    public int calculateWeaponEXP(int hitsLanded)
    {
        return hitsLanded * 2;
    }

    public int calculateEXP(Unit enemy, bool isKilled, bool hitLanded, int assassinateBonus)
    {
        int xp = 0;

        if (hitLanded)
        {
            xp += (31 + (enemy.level + enemy.unitClass.classBonusA) - (level + unitClass.classBonusA)) / unitClass.classPower;
        }
        else 
        {
            return 1;
        }

        if (isKilled)
        {
            int bossBonus = enemy.isBoss ? 40 : 0;
            int thiefBonus = unitClass.isThief ? 20 : 0;
            int baseKill = ((enemy.level * enemy.unitClass.classPower) + enemy.unitClass.classBonusB) - (((level * unitClass.classPower) + unitClass.classBonusB) / 1);

            if (baseKill < 1)
            {
                baseKill = ((enemy.level * enemy.unitClass.classPower) + enemy.unitClass.classBonusB) - (((level * unitClass.classPower) + unitClass.classBonusB) / 2);
            }
            
            xp += baseKill + 20 + bossBonus + thiefBonus;
        }

        return xp * assassinateBonus;
    }
}