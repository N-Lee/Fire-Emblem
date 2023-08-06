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
        maxHp = 16;
        currentHp = 16;
        might = 4;
        skill = 7;
        speed = 9;
        luck = 5;
        defense = 2;
        resistance = 0;
        constitution = 5;
        move = 5;
        terrain = 0;
        SetClass(new LordClass(), false);

        characterOffset = new Vector3(0.6f,0.062f,0f);
        equippedWeapon = new IronSword();

        base.Start();

        Weapon ironSword = new IronSword();
        Weapon ironAxe = new IronAxe();
        inventory.AddWeapon(ironSword);
        inventory.AddWeapon(ironAxe);
        equippedWeapon = inventory.EquippedWeapon();
    }


}
