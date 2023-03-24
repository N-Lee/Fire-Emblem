using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{

    public TileBase[] tiles;

    public int moveCost;
    public bool isWalkable;
    public int evasionBonus;

}
