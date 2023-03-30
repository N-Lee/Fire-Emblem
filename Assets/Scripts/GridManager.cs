using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum UserPhase {Map, CharacterMove, CharacterAction, ActionTarget, Battle};

public class GridManager : MonoBehaviour
{
    UserPhase userPhase;
    [SerializeField] CursorController cursorController;
    [SerializeField] Tilemap groundTilemap, moveTilemap;
    [SerializeField] List<TileData> tileDatas;
    [SerializeField] List<Tile> tiles;
    Dictionary<TileBase, TileData> dataFromTiles;
    HashSet<Node> movementNodes = new HashSet<Node>();
    AStar astar;
    MovementController movementController;
    Vector3Int startPos, goalPos;
    bool isArrowDrawn = false;

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
        astar.groundTilemap = groundTilemap;
        astar.dataFromTiles = dataFromTiles;

        movementController = new MovementController();
        movementController.groundTilemap = groundTilemap;
        movementController.dataFromTiles = dataFromTiles;
    }

    // Update is called once per frame
    void Update()
    {
        MouseClick();

        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    void MouseClick()
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

    void MapPhase(Vector3 mousePos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePos);

            if (targetObject && targetObject.transform.gameObject.tag == "Player")
            {
                startPos = moveTilemap.WorldToCell(mousePos);
                movementController.startPos = startPos;
                movementNodes = movementController.GetMovementTiles(5);
                userPhase = UserPhase.CharacterMove;
                cursorController.ShowCursor(false);
            }
            else
            {
                Vector3Int gridPosition = groundTilemap.WorldToCell(mousePos);
                TileBase clickedTile = groundTilemap.GetTile(gridPosition);
                bool isWalkable = dataFromTiles[clickedTile].isWalkable;
            }
        }
    }

    void CharacterMovePhase(Vector3 mousePos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(mousePos);
        Node currentNode = movementController.GetNode(gridPosition);

        if (gridPosition != goalPos && movementNodes.Contains(currentNode) && startPos != gridPosition)
        {
            if (isArrowDrawn)
            {
                astar.Reset();
                isArrowDrawn = false;
            }

            goalPos = gridPosition;

            TileBase hoveredTile = groundTilemap.GetTile(gridPosition);
            astar.Algorithm(startPos, goalPos, tiles);
            isArrowDrawn = true;
        }


        if (Input.GetMouseButtonDown(0))
        {
            userPhase = UserPhase.Map;
        }
    }

    void DebugAStar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           astar.Algorithm(startPos, goalPos, tiles);
        }
    }
}
