using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordClass : UnitClass
{
    // Start is called before the first frame update
    void Start()
    {
        baseHp = 16;
        basestrength = 4;
        baseSkill = 7;
        baseSpeed = 9;
        baseLuck = 5;
        baseDefense = 2;
        baseResistance = 0;
        baseConstitution = 5; 
        baseMove = 5;

        maxHp = 20;
        maxstrength = 20;
        maxSkill = 20;
        maxSpeed = 20;
        maxLuck = 30;
        maxDefense = 20;
        maxResistance = 20;
        maxConstitution = 20;
        maxMove = 15;

        growthHp = 90;
        growthstrength = 45;
        growthSkill = 40;
        growthSpeed = 45;
        growthLuck = 45;
        growthDefense = 15;
        growthResistance = 20;

        isPromoted = false;
        isThief = false;
        classPower = 3;
        classBonusA = 0;
        classBonusB = 0;

        useableWeapon = new Dictionary<WeaponType, int>();
        useableWeapon.Add(WeaponType.sword, 31);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
