using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile tile;
    [SerializeField] Color openColour, closedColour, pathColour, currentColour, startColour, goalColour;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject debugTextPrefeb;
    List<GameObject> debugObjects = new List<GameObject>();
    static AStarDebugger instance;
    
    public static AStarDebugger myInstance
    {
        get 
        {
            if (instance == null)
            {
                instance  = FindAnyObjectByType<AStarDebugger>();
            }
            
            return instance;
        }
    }

    public void CreateTiles (HashSet<Node> openList, HashSet<Node> closedList, Dictionary<Vector3Int, Node> allNodes, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach (Node node in openList)
        {
            ColourTile(node.Position, openColour);
        }

        foreach (Node node in closedList)
        {
            ColourTile (node.Position, closedColour);
        }

        if (path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColourTile(pos, pathColour);
                }
            }
        }
        

        ColourTile (start, startColour);
        ColourTile (goal, goalColour);

        foreach (KeyValuePair<Vector3Int,Node> node in allNodes)
        {
            if (node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefeb, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
        }
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {
        debugText.P.text = $"F:{node.Position.x},{node.Position.y}";
        debugText.F.text = $"F:{node.F}";
        debugText.G.text = $"G:{node.G}";
        debugText.H.text = $"H:{node.H}";

        Vector3Int diretion = node.Parent.Position - node.Position;
        debugText.MyArrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg);
    }

    public void ColourTile(Vector3Int position, Color colour)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, colour);
    }


}
