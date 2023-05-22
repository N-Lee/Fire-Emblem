using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lyn : Unit 
{

    // Start is called before the first frame update
    protected override void Start()
    {
        level = 1;
        exp = 0;
        hp = 16;
        strength = 4;
        skill = 7;
        speed = 9;
        luck = 5;
        defense = 2;
        resistance = 0;
        constitution = 5;
        move = 5;
        SetClass(new LordClass(), false);

        characterOffset = new Vector3(0.6f,0.062f,0f);
        equippedWeapon = new IronSword();

        base.Start();
    }

    // Update is called once per frame
    
}
