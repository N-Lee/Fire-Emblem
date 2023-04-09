using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lyn : Unit 
{

    // Start is called before the first frame update
    void Start()
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

        base.Start();
    }

    // Update is called once per frame
    
}
