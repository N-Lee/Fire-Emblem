using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform movePoint;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Grid grid;
    [SerializeField] Tilemap cursorMap = null;
    [SerializeField] Tilemap collisionMap = null;
    [SerializeField] Tile cursorTile = null;
    Vector3Int previousMousePos = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMove();
    }

    #region Controller Option
    void ControllerMove()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Mouse Option
    void MouseMove()
    {
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos))
        {
            cursorMap.SetTile(previousMousePos, null);
            cursorMap.SetTile(mousePos, cursorTile);
            previousMousePos = mousePos;
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
    #endregion
}
