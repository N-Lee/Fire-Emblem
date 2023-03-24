using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    
    [SerializeField] private Tilemap ground;
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;


    // Start is called before the first frame update
    void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = ground.WorldToCell(mouseWorldPos);
            
            TileBase clickedTile = ground.GetTile(gridPosition);

            bool isWalkable = dataFromTiles[clickedTile].isWalkable;

            Debug.Log("Is " + clickedTile + " walkable: " + isWalkable);
        }
    }
}
