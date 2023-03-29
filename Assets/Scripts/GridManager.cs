using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum UserPhase {Map, CharacterMove, CharacterAction, ActionTarget, Battle};

public class GridManager : MonoBehaviour
{
    UserPhase userPhase;
    [SerializeField] CursorController cursorController;
    [SerializeField] Tilemap groundTileMap, debugTileMap;
    [SerializeField] List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;
    AStar astar;
    MovementController movementController;
    Vector3Int startPos, goalPos;

    // Start is called before the first frame update
    void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        userPhase = UserPhase.Map;

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        astar = new AStar();
        astar.groundTileMap = groundTileMap;
        astar.dataFromTiles = dataFromTiles;

        movementController = new MovementController();
        movementController.groundTilemap = groundTileMap;
        movementController.dataFromTiles = dataFromTiles;
    }

    // Update is called once per frame
    void Update()
    {
        MouseClick();

        if (Input.GetKeyDown(KeyCode.Space))
        {
           astar.Algorithm(startPos, goalPos);
        }
    }

    void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            

            switch (userPhase)
            {
                case UserPhase.Map:
                    MapPhase(mousePos);
                    break;
                
                case UserPhase.CharacterMove:
                    CharacterMovePhase(mousePos);
                    break;

                default:
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
        }
    }

    void MapPhase(Vector3 mousePos)
    {
        Collider2D targetObject = Physics2D.OverlapPoint(mousePos);

        if (targetObject && targetObject.transform.gameObject.tag == "Player")
        {
            Debug.Log(targetObject.transform.gameObject.name);
        }
        else
        {
            Vector3Int gridPosition = groundTileMap.WorldToCell(mousePos);
            TileBase clickedTile = groundTileMap.GetTile(gridPosition);
            bool isWalkable = dataFromTiles[clickedTile].isWalkable;

            //Debug.Log("Is " + clickedTile + " walkable: " + isWalkable);
            Debug.Log("Clicked: " + gridPosition);
        }

        startPos = debugTileMap.WorldToCell(mousePos);
        movementController.startPos = startPos;
        movementController.Algorithm(3);

        userPhase = UserPhase.CharacterMove;
    }

    void CharacterMovePhase(Vector3 mousePos)
    {
        goalPos = debugTileMap.WorldToCell(mousePos);

        Vector3Int gridPosition = groundTileMap.WorldToCell(mousePos);
        TileBase clickedTile = groundTileMap.GetTile(gridPosition);

        userPhase = UserPhase.Map;
        cursorController.isSelected = true;
    }

}
