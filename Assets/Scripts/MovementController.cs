using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

enum MoveOptions {Attack, Move, Remove};

public class MovementController : MonoBehaviour
{
    public Tilemap groundTilemap {get; set;}
    public Vector3Int startPos {get; set;}
    HashSet<Node> movementList, maxRangeList, attackList;
    Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    Node current;
    public Dictionary<TileBase, TileData> dataFromTiles {get; set;}

    Color moveColor = new Color32(0,0,255,130);
    Color attackColor = new Color32(255,0,0,130);
    Color assistColor = new Color32(0,255,0,130);
    Color defaultColor = new Color32(255,255,255,0);

    // Initialize variables
    private void Initialize()
    {
        current = GetNode(startPos);

        movementList = new HashSet<Node>();
        movementList.Add(current);
        maxRangeList = new HashSet<Node>();
        attackList = new HashSet<Node>();
    }

    // Reset variables and remove colours in tiles
    public void Reset()
    {

        current = null;

        MovementDraw.myInstance.ColourMove(movementList, defaultColor);
        MovementDraw.myInstance.ColourMove(attackList, defaultColor);

        foreach(Node node in movementList)
        {
            node.G = 0;
            node.F = 0;
            node.H = 0;
            node.Parent = null;
        }

        movementList.Clear();
        maxRangeList.Clear();
        attackList.Clear();
    }

    #region Character Move and Attack
    // Finds and colours tiles for movement and attack
    public HashSet<Node> GetMovementTiles(int unitMaxMove)
    {
        Initialize();
        FindMovementTiles(unitMaxMove);

        MovementDraw.myInstance.ColourMove(movementList, moveColor);
        return movementList;
    }

    public HashSet<Node> GetAttackTiles(int minAttackRange, int maxAttackRange)
    {
        FindAttackTiles(minAttackRange, maxAttackRange);
        attackList.ExceptWith(movementList);

        MovementDraw.myInstance.ColourMove(attackList, attackColor);
        return attackList;
    }

    // Finds the tiles the character can move to
    private void FindMovementTiles(int unitMaxMove)
    {
        HashSet<Node> UIList = new HashSet<Node>();
        HashSet<Node> tempUIList = new HashSet<Node>();
        List<Node> neighbours = FindMovementNeighbours(current.Position);

        // Find the neighbours from initial position
        foreach (Node neighbour in neighbours)
        {
            int g = DetermineG(neighbour.Position, current.Position);
            if (unitMaxMove - (neighbour.G + g) >= 0)
            {
                CalculateValues(current, neighbour, g);
                UIList.Add(neighbour);
            }
        }
        movementList.UnionWith(UIList);
        
        // UIList contains tiles where the neighbours have not been discovered
        // UIList will be empty once there are no more valid neighbours
        while (UIList.Count > 0)
        {
            tempUIList = new HashSet<Node>();
            foreach(Node finalizedNode in UIList)
            {
                neighbours = FindMovementNeighbours(finalizedNode.Position);
                foreach (Node neighbour in neighbours)
                {
                    int g = DetermineG(neighbour.Position, finalizedNode.Position);
                    // If it is a previously reached tile and is a more efficient path, add it back to the list
                    if ((movementList.Contains(neighbour) || tempUIList.Contains(neighbour)) && finalizedNode.G + g < neighbour.G)
                    {                       
                        CalculateValues(finalizedNode, neighbour, g);
                        tempUIList.Add(finalizedNode);
                    }
                    // If it is within the characters move range and a new tile, add to list
                    else if (unitMaxMove - (finalizedNode.G + g) >= 0 && !tempUIList.Contains(neighbour) && !UIList.Contains(neighbour) && !movementList.Contains(neighbour))
                    {
                        CalculateValues(finalizedNode, neighbour, g);
                        tempUIList.Add(neighbour);
                    }

                    // If it is the farthest the character can move, add it to the maxRangeList. Will be used to calculate attack range
                    if (finalizedNode.G == unitMaxMove
                    || (g > 1 && finalizedNode.G + g >= unitMaxMove))
                    {
                        maxRangeList.Add(finalizedNode);
                    }
                }
            }
            
            UIList = tempUIList;
            movementList.UnionWith(UIList);
        }
    }

    // Given a position, finds the adjacent neighbouring tiles the character can move to
    private List<Node> FindMovementNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();
        
        // Will only accept adjacent tiles. Diagonals won't work
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 ^ y == 0)
                {
                    Vector3Int neighbourPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    TileBase neighbourTile = groundTilemap.GetTile(neighbourPos);
                    Collider2D targetObject = Physics2D.OverlapPoint(new Vector2 (neighbourPos.x+0.1f, neighbourPos.y+0.1f));

                    // If it is not at the starting position, within the map, and walkable
                    if (neighbourPos != startPos
                    && groundTilemap.GetTile(neighbourPos)
                    && dataFromTiles[neighbourTile].isWalkable)
                    {
                        Node neighbour = GetNode(neighbourPos);
                        // Add to attacklist if it is an enemy
                        if (targetObject && targetObject.transform.gameObject.tag == "Enemy")
                        {
                            attackList.Add(neighbour);
                        }
                        // Add to neighbours if the target object is not a player or enemy
                        else if (!(targetObject && (targetObject.transform.gameObject.tag == "Player" || targetObject.transform.gameObject.tag == "Enemy")))
                        {
                            neighbours.Add(neighbour);
                        }
                        
                    }
                }
            }
        }

        return neighbours;
    }

    // After finding the movement of a player, finds the tiles that the user can attack on
    public void FindAttackTiles(int minAttackRange, int maxAttackRange)
    {
        foreach(Node node in movementList)
        {
            
            HashSet<Node> UIList;
            UIList = new HashSet<Node>();
            UIList.Add(node);
            List<Node> neighbours = new List<Node>();
            HashSet<Node> oneTileAttackList = new HashSet<Node>();
            HashSet<Node> seenList = new HashSet<Node>();
            HashSet<Node> tempUIList = new HashSet<Node>();

            for (int i = 1; i <= maxAttackRange; i++)
            {
                foreach(Node edgeNode in UIList)
                {
                    neighbours = FindAttackNeighbours(edgeNode.Position);

                    foreach(Node neighbour in neighbours)
                    {
                            if (i >= minAttackRange)
                            {
                                oneTileAttackList.Add(neighbour);
                            }

                            if (i < minAttackRange)
                            {
                                seenList.Add(neighbour);
                            }

                            tempUIList.Add(neighbour);
                    }
                }
                UIList = tempUIList;
                tempUIList = new HashSet<Node>();
            }

            oneTileAttackList.ExceptWith(seenList);
            attackList.UnionWith(oneTileAttackList);
        }
    }

    // Given a position, finds the adjacent neighbouring tiles that can be attacked
    private List<Node> FindAttackNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 ^ y == 0)
                {
                    Vector3Int neighbourPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    TileBase neighbourTile = groundTilemap.GetTile(neighbourPos);

                    if (groundTilemap.GetTile(neighbourPos))
                    {
                        Node neighbour = GetNode(neighbourPos);
                        neighbours.Add(neighbour);
                    }
                }
            }
        }

        return neighbours;
    }

    // Calculates the movement cost from start tile. Reused G from AStar to represent movement cost
    private void CalculateValues(Node parent, Node neighbour, int cost)
    {
        neighbour.Parent = parent;
        neighbour.G = parent.G + cost;
    }

    // Calculates the movement cost from parent tile
    private int DetermineG (Vector3Int neighbour, Vector3Int current)
    {
        int g = 0;

        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        TileBase neighbourTile = groundTilemap.GetTile(neighbour);
        int moveCost = dataFromTiles[neighbourTile].moveCost;

        if (Mathf.Abs(x-y) % 2 == 1)
        {
            g = moveCost;
        }

        return g;
    }

    // Given a position, find the node
    public Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else 
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
    #endregion

}
