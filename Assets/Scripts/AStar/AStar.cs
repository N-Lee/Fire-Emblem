using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : Tile
{
    public Tilemap groundTileMap {get; set;}
    public Vector3Int startPos {get; set;}
    public Vector3Int goalPos {get; set;}
    HashSet<Node> openList, closedList;
    Stack<Vector3Int> path;
    Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    Node currentNode;
    public Dictionary<TileBase, TileData> dataFromTiles {get; set;}

    private void Initialize()
    {
        currentNode = GetNode(startPos);

        openList = new HashSet<Node>();
        openList.Add(currentNode);
        closedList = new HashSet<Node>();
    }

    public void Algorithm(Vector3Int start, Vector3Int goal)
    {
        if (currentNode == null)
        {
            startPos = start;
            goalPos = goal;
            Initialize();
        }
        else
        {
            openList.Clear();
            closedList.Clear();
        }

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(currentNode.Position);
            ExamineNeighbours(neighbours, currentNode);

            UpdateCurrentTile(ref currentNode);
            path = GeneratePath(currentNode);
        }

        AStarDebugger.myInstance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);
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
                    TileBase neighbourTile = groundTileMap.GetTile(neighbourPos);

                    if (neighbourPos != startPos
                    && groundTileMap.GetTile(neighbourPos)
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

        TileBase neighbourTile = groundTileMap.GetTile(neighbour);
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

}
