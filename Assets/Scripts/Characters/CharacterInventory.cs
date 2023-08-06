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
        weapons = new List<Weapon>();
    }

    public void AddWeapon(Weapon newWeapon)
    {
        weapons.Add(newWeapon);
    }

    public void EquipWeapon(Weapon selectedWeapon)
    {
        int index = weapons.IndexOf(selectedWeapon);
        weapons.RemoveAt(index);
        weapons.Insert(0, selectedWeapon);
        unit.equippedWeapon = weapons[0];
    }

    public Weapon EquippedWeapon()
    {
        return weapons[0];
    }
}
