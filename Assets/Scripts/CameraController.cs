using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] Tilemap groundTileMap;
    [SerializeField] Transform northDetect;
    [SerializeField] Transform eastDetect;
    [SerializeField] Transform southDetect;
    [SerializeField] Transform westDetect;
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
        Vector3Int northDetectInt = new Vector3Int(Mathf.FloorToInt(northDetect.position.x), Mathf.FloorToInt(northDetect.position.y), 0);
        Vector3Int eastDetectInt = new Vector3Int(Mathf.FloorToInt(eastDetect.position.x), Mathf.FloorToInt(eastDetect.position.y), 0);
        Vector3Int southDetectInt = new Vector3Int(Mathf.FloorToInt(southDetect.position.x), Mathf.FloorToInt(southDetect.position.y), 0);
        Vector3Int westDetectInt = new Vector3Int(Mathf.FloorToInt(westDetect.position.x), Mathf.FloorToInt(westDetect.position.y), 0);

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
