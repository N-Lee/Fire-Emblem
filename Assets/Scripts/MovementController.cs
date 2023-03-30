using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

enum MoveOptions {Attack, Move, Remove};

public class MovementController : MonoBehaviour
{
    [SerializeField] Grid grid;    
    [SerializeField] Tile arrowTile;
    Color moveColour = new Color(0,0,255,130);
    Color attackColour = new Color (255,0,0,130);
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject debugTextPrefeb;
    [SerializeField] Tilemap moveTilemap; 

    public Tilemap groundTilemap {get; set;}
    public Vector3Int startPos {get; set;}
    public Vector3Int goalPos {get; set;}
    HashSet<Node> openList, closedList, UIList, tempUIList, finalList;
    Stack<Vector3Int> path;
    Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    Node current;
    public Dictionary<TileBase, TileData> dataFromTiles {get; set;}

    private void Initialize()
    {
        current = GetNode(startPos);

        finalList = new HashSet<Node>();
        finalList.Add(current);
        UIList = new HashSet<Node>();
        tempUIList = new HashSet<Node>();
    }

    public void ClearData()
    {
        current = null;
        finalList = new HashSet<Node>();
        UIList = new HashSet<Node>();
        tempUIList = new HashSet<Node>();
    }

    #region Character Move
    public HashSet<Node> GetMovementTiles(int unitMaxMove)
    {
        Initialize();

        List<Node> neighbours = FindNeighbours(current.Position);
        foreach (Node neighbour in neighbours)
        {
            int g = DetermineG(neighbour.Position, current.Position);
            if (unitMaxMove - (neighbour.G + g) >= 0)
            {
                CalculateValues(current, neighbour, g);
                UIList.Add(neighbour);
            }
        }
        finalList.UnionWith(UIList);

        while (UIList.Count > 0)
        {
            tempUIList = new HashSet<Node>();
            foreach(Node finalizedNode in UIList)
            {
                neighbours = FindNeighbours(finalizedNode.Position);
                foreach (Node neighbour in neighbours)
                {
                    int g = DetermineG(neighbour.Position, finalizedNode.Position);
                    if ((finalList.Contains(neighbour) || tempUIList.Contains(neighbour)) && finalizedNode.G + g < neighbour.G)
                    {                       
                        CalculateValues(finalizedNode, neighbour, g);
                        tempUIList.Add(finalizedNode);
                    }
                    else if (unitMaxMove - (finalizedNode.G + g) >= 0 && !tempUIList.Contains(neighbour) && !UIList.Contains(neighbour) && !finalList.Contains(neighbour))
                    {
                        CalculateValues(finalizedNode, neighbour, g);
                        tempUIList.Add(neighbour);
                    }
                }
            }
            UIList = tempUIList;
            finalList.UnionWith(UIList);
        }

        MovementDraw.myInstance.ColourMove(finalList);
        return finalList;
    }

    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 || y == 0)
                {
                    Vector3Int neighbourPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    TileBase neighbourTile = groundTilemap.GetTile(neighbourPos);

                    if (neighbourPos != startPos
                    && groundTilemap.GetTile(neighbourPos)
                    && dataFromTiles[neighbourTile].isWalkable)
                    {
                        Node neighbour = GetNode(neighbourPos);
                        neighbours.Add(neighbour);
                    }
                }
            }
        }

        return neighbours;
    }

    private void CalculateValues(Node parent, Node neighbour, int cost)
    {
        neighbour.Parent = parent;
        neighbour.G = parent.G + cost;
    }

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

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

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

    Stack<Vector3Int> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while (current.Position != startPos)
            {
                finalPath.Push(current.Position);

                current = current.Parent;
            }

            return finalPath;
        }

        return null;
    }
    #endregion

}
