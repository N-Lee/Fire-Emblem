using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementDraw : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Tilemap moveTilemap;
    [SerializeField] Tile arrowTile;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject MovementPrefab;
    Color moveColor = new Color32(0,0,255,130);
    Color attackColor = new Color(255,0,0,130);
    static MovementDraw instance;
    List<GameObject> arrowObjects = new List<GameObject>();
    
    public static MovementDraw myInstance
    {
        get 
        {
            if (instance == null)
            {
                instance  = FindAnyObjectByType<MovementDraw>();
            }
            
            return instance;
        }
    }

    public void ColourMove(HashSet<Node> list)
    {
        foreach (Node node in list)
        {
            ColourTile(node.Position, moveColor);
        }
    }

    public void ColourTile(Vector3Int position, Color colour)
    {
        moveTilemap.SetTile(position, arrowTile);
        moveTilemap.SetTileFlags(position, TileFlags.None);
        moveTilemap.SetColor(position, colour);
    }

    public void DrawArrows(Dictionary<Vector3Int, Node> path)
    {
        foreach (KeyValuePair<Vector3Int,Node> node in path)
        {
            if (node.Value.Parent != null)
            {
                GameObject go = Instantiate(MovementPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                arrowObjects.Add(go);
                GenerateArrow(node.Value, go.GetComponent<MovementArrow>());
            }
        }
    }

    private void GenerateArrow(Node node, MovementArrow movementArrow)
    {
        Vector3Int diretion = node.Parent.Position - node.Position;
        movementArrow.MyArrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg);
    }

    public void ResetAStar()
    {
        foreach (GameObject go in arrowObjects)
        {
            Destroy(go);
        }

        arrowObjects.Clear();
    }

    #region A* Debug variables and functions

    Color openColour, closedColour, pathColour, currentColour, startColour, goalColour; // Add serialize field when want to use
    List<GameObject> debugObjects = new List<GameObject>();
    GameObject debugTextPrefeb; // Add serialize field when want to use

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
    #endregion
}
