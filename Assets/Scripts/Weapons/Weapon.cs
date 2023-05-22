using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {sword, axe, lance, staff, tome, empty};
public class Weapon
{
    public int minRange, maxRange, durabilty, weight, might, hitRate, critRate, weaponExp, cost;
    public char rank;
    public string naming;
    public bool isPhysical;
    public WeaponType weaponType;
}
