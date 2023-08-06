using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitClass : MonoBehaviour
{
    public string name;
    public int baseHp, basestrength, baseSkill, baseSpeed, baseLuck, baseDefense, baseResistance, baseConstitution, baseMove;
    public int maxHp, maxstrength, maxSkill, maxSpeed, maxLuck, maxDefense, maxResistance, maxConstitution, maxMove;
    public int growthHp, growthstrength, growthSkill, growthSpeed, growthLuck, growthDefense, growthResistance;
    public string[] skill;
    public bool isNPC;
    public bool isPromoted;
    public bool isThief;
    public int classPower; // 1 for Bern Prince, Civilian, Transporter (Tent and Wagon), 2 for Clerics, Soldiers, Troubadours, Bards, Thieves and Dancers, 5 for the Fire Dragon, 3 for everything else.
    public int classBonusA; // 0 for non-promoted classes, 20 for promoted classes.
    public int classBonusB; // 0 for non-promoted classes, 40 for Assassins, Bishop (F) and Valkyries, 60 for all other promoted classes.
    public Dictionary<WeaponType, int> useableWeapon; // int is for the weapon exp needed. Start at 1 (E). 31 is D. 71 is C (+40). 121 is B. 181 is A. 251 is S 
}
