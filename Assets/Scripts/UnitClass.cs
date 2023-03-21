using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitClass : MonoBehaviour
{
    public int baseLevel, baseHp, baseSm, baseSkill, baseSpeed, baseLuck, baseDefense, baseResistance, baseConstitution, baseMove;
    public int maxLevel, maxHp, maxSm, maxSkill, maxSpeed, maxLuck, maxDefense, maxResistance, maxConstitution, maxMove;
    public int growthLevel, growthHp, growthSm, growthSkill, growthSpeed, growthLuck, growthDefense, growthResistance, growthConstitution;
    public string skill;
    public bool isPhysical;
    public bool isThief;
    public int classPower; // 1 for Bern Prince, Civilian, Transporter (Tent and Wagon), 2 for Clerics, Soldiers, Troubadours, Bards, Thieves and Dancers, 5 for the Fire Dragon, 3 for everything else.
    public int classBonusA; // 0 for non-promoted classes, 20 for promoted classes.
    public int classBonusB; // 0 for non-promoted classes, 40 for Assassins, Bishop (F) and Valkyries, 60 for all other promoted classes.
    public List<string> weapons;
    public List<int> baseWeaponLevel;
    public List<int> maxWeaponLevel;
}
