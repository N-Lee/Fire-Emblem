using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : Tile
{
    List<Tile> tiles;
    public Tilemap groundTilemap {get; set;}
    public Vector3Int startPos {get; set;}
    public Vector3Int goalPos {get; set;}
    HashSet<Node> openList, closedList;
    HashSet<Vector3Int> changedTiles = new HashSet<Vector3Int>();
    Dictionary<Vector3Int, Node> path;
    Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    Node current;
    public Dictionary<TileBase, TileData> dataFromTiles {get; set;}

    private void Initialize()
    {
        current = GetNode(startPos);

        openList = new HashSet<Node>();
        openList.Add(current);
        closedList = new HashSet<Node>();
    }

    public void Algorithm(Vector3Int start, Vector3Int goal, List<Tile> tiles)
    {
        if (current == null)
        {
            startPos = start;
            goalPos = goal;
            Initialize();
            this.tiles = tiles;
        }

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(current.Position);
            ExamineNeighbours(neighbours, current);

            UpdateCurrentTile(ref current);
            path = GeneratePath(current);
        }

        MovementDraw.myInstance.DrawArrows(path);
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

    private void ExamineNeighbours(List<Node> neighbours, Node current)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = neighbours[i];

            int g = DetermineG(neighbours[i].Position, current.Position);

            if (openList.Contains(neighbour))
            {
                if (current.G + g < neighbour.G)
                {
                    CalculateValues(current, neighbour, g);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                CalculateValues(current, neighbour, g);
                openList.Add(neighbour);
            }
        }
    }

    private void CalculateValues(Node parent, Node neighbour, int cost)
    {
        neighbour.Parent = parent;

        neighbour.G = parent.G + cost;
        neighbour.H = (Mathf.Abs(neighbour.Position.x - goalPos.x) + Mathf.Abs(neighbour.Position.y - goalPos.y)) * 10;
        neighbour.F = neighbour.G + neighbour.H;
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
            g = 10 * moveCost;
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

    private Node GetNode(Vector3Int position)
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

    Dictionary<Vector3Int, Node> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Dictionary<Vector3Int, Node> finalPath = new Dictionary<Vector3Int, Node>();

            while (current.Position != startPos)
            {
                finalPath.Add(current.Position, current);
                current = current.Parent;
            }

            return finalPath;
        }

        return null;
    }

    public void Reset()
    {
        MovementDraw.myInstance.ResetAStar();

        foreach (Vector3Int position in path.Keys)
        {
            groundTilemap.SetTile(position, tiles[0]);
        }

        allNodes.Clear();
        path = null;
        current = null;
    }
}
