using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brigand : Unit 
{
    // Start is called before the first frame update
    protected override void Start()
    {
        level = 1;
        exp = 0;
        hp = 20;
        strength = 5;
        skill = 1;
        speed = 5;
        luck = 0;
        defense = 3;
        resistance = 0;
        constitution = 12;
        move = 5;

        characterOffset = new Vector3(0.6f,0f,0f);
        equippedWeapon = new IronSword();

        base.Start();
    }
}
