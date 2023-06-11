using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    readonly int MAXITEMS = 5;
    Unit unit;
    public List<Weapon> weapons;

    public CharacterInventory(Unit unit)
    {
        this.unit = unit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EquipWeapon()
    {
        
    }
}
