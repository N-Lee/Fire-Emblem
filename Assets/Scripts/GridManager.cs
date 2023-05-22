using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO: Finish Brigand mapping

enum UserPhase {Map, CharacterMove, Menu, Action, Battle};
public enum ActionMenuOptions{Attack, Staff, Rescue, Item, Trade, Wait};

public class GridManager : MonoBehaviour
{
    [SerializeField] UserPhase userPhase;
    ActionMenuOptions actionMenuOptions;
    [SerializeField] CursorController cursorController;
    [SerializeField] Tilemap groundTilemap, moveTilemap;
    [SerializeField] List<TileData> tileDatas;
    [SerializeField] GameObject actionMenuObj;
    ActionMenu actionMenu;
    Dictionary<TileBase, TileData> dataFromTiles;
    Unit selectedCharacter;
    HashSet<Node> movementNodes = new HashSet<Node>();
    HashSet<Node> attackNodes = new HashSet<Node>();
    List<Node> path = new List<Node>();
    AStar astar;
    MovementController movementController;
    Vector3Int startPos, goalPos;
    bool isArrowDrawn = false;
    bool isMoving = false;

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

        movementController = gameObject.AddComponent<MovementController>();
        movementController.groundTilemap = groundTilemap;
        movementController.dataFromTiles = dataFromTiles;

        actionMenu = actionMenuObj.GetComponent<ActionMenu>();
    }

    // Update is called once per frame
    void Update()
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

            case UserPhase.Menu:
                MenuPhase();
                break;

            case UserPhase.Action:
                ActionPhase(mousePos);
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
                selectedCharacter = targetObject.transform.gameObject.GetComponent<Unit>();

                startPos = moveTilemap.WorldToCell(mousePos);
                movementController.startPos = startPos;
                movementNodes = movementController.GetMovementTiles(selectedCharacter.move);
                attackNodes = movementController.GetAttackTilesDuringMove(selectedCharacter.equippedWeapon.minRange, selectedCharacter.equippedWeapon.maxRange);

                cursorController.ShowCursor(false);
                userPhase = UserPhase.CharacterMove;
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
        Collider2D targetObject = Physics2D.OverlapPoint(mousePos);

        if (!isMoving)
        {
            Vector3Int gridPosition = groundTilemap.WorldToCell(mousePos);
            Node currentNode = movementController.GetNode(gridPosition);

            if (gridPosition != goalPos && movementNodes.Contains(currentNode) && startPos != gridPosition)
            {
                RemoveArrow();

                goalPos = gridPosition;

                TileBase hoveredTile = groundTilemap.GetTile(gridPosition);
                path = astar.Algorithm(startPos, goalPos);
                isArrowDrawn = true;
            }


            if (Input.GetMouseButtonDown(0) && movementNodes.Contains(currentNode))
            {
                if (goalPos == startPos)
                {
                    userPhase = UserPhase.Menu;
                    return;
                }

                selectedCharacter.SetPath(path);
                RemoveMovementUI();
                isMoving = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                DeselectCharacter();
                RemoveMovementUI();
                selectedCharacter = null;
                userPhase = UserPhase.Map;
            }
        }

        if (isMoving && !selectedCharacter.isCharacterMoving)
        {
            userPhase = UserPhase.Menu;
        }
    }
    
    void MenuPhase()
    {
        List<string> menuOptions = new List<string>();
        if (selectedCharacter.equippedWeapon.weaponType != WeaponType.empty)
        {
            menuOptions.Add("Attack");
        }

        menuOptions.Add("Wait");

        actionMenu.Show(menuOptions);
        actionMenu.MoveMenu(Camera.main.WorldToScreenPoint(selectedCharacter.gameObject.transform.position));
    }

    void ActionPhase(Vector3 mousePos)
    {
        Collider2D targetObject = Physics2D.OverlapPoint(mousePos);

        switch(actionMenuOptions)
        {
            case ActionMenuOptions.Attack:
                if (Input.GetMouseButtonDown(0) && targetObject && targetObject.transform.gameObject.tag == "Enemy")
                {
                    Debug.Log("Attack Unit");
                }
                else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    RemoveMovementUI();
                    userPhase = UserPhase.CharacterMove;
                }
                break;

            case ActionMenuOptions.Staff:
                break;

            case ActionMenuOptions.Rescue:
                break;

            case ActionMenuOptions.Item:
                break;

            case ActionMenuOptions.Wait:
                break;
        }
    }

    void DeselectCharacter()
    {
        selectedCharacter = null;
        movementNodes.Clear();
        attackNodes.Clear();
        path.Clear();
    }

    void RemoveArrow()
    {
        if (isArrowDrawn)
        {
            astar.Reset();
            isArrowDrawn = false;
        }
    }

    void RemoveMovementUI()
    {
        RemoveArrow();
        movementController.Reset();
    }

    public void AttackButton()
    {
        movementController.GetAttackTilesDuringAttack(1,1,moveTilemap.WorldToCell(selectedCharacter.gameObject.transform.position));
        actionMenu.Hide();
        actionMenuOptions = ActionMenuOptions.Attack;
        userPhase = UserPhase.Action;
    }

    void DebugAStar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           astar.Algorithm(startPos, goalPos);
        }
    }
}
