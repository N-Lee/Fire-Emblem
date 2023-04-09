using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] Tilemap groundTileMap;
    Camera cam;
    float moveSpeed = 10f;
    float edgeOffset = 30f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        EdgePan();
    }

    void EdgePan()
    {   
        Vector3 north = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height, 0));
        Vector3 east = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height/2, 0));
        Vector3 south = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, 0, 0));
        Vector3 west = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height/2, 0));
        
        Vector3Int northDetectInt = new Vector3Int(Mathf.FloorToInt(north.x), Mathf.FloorToInt(north.y), 0);
        Vector3Int eastDetectInt = new Vector3Int(Mathf.FloorToInt(east.x), Mathf.FloorToInt(east.y), 0);
        Vector3Int southDetectInt = new Vector3Int(Mathf.FloorToInt(south.x), Mathf.FloorToInt(south.y-1), 0);
        Vector3Int westDetectInt = new Vector3Int(Mathf.FloorToInt(west.x-1), Mathf.FloorToInt(west.y), 0);

        if (Input.mousePosition.x > Screen.width - edgeOffset &&
        groundTileMap.GetTile<Tile>(eastDetectInt) != null)
        {
            transform.position += new Vector3 (1f, 0f, 0f);
        }

        if (Input.mousePosition.x < edgeOffset &&
        groundTileMap.GetTile<Tile>(westDetectInt) != null)
        {
            transform.position -= new Vector3 (1f, 0f, 0f);
        }

        if (Input.mousePosition.y > Screen.height- edgeOffset &&
        groundTileMap.GetTile<Tile>(northDetectInt) != null)
        {
            transform.position += new Vector3 (0f, 1f, 0f);
        }

        if (Input.mousePosition.y < edgeOffset &&
        groundTileMap.GetTile<Tile>(southDetectInt) != null)
        {
            transform.position -= new Vector3 (0f, 1f, 0f);
        }
    }
}
