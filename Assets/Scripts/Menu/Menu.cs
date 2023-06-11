using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    RectTransform rectTransform;
    Vector3 menuOffset = new Vector3(30, -200, 0);
    float leftXOffset = 40f;
    
    public void MoveMenu(Vector3 unitPosition)
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        if (Screen.width/2 > unitPosition.x)
        {
            rectTransform.anchorMax = new Vector2(1,1);
            rectTransform.anchorMin = new Vector2(1,1);
            rectTransform.pivot = new Vector2(1,1);
            rectTransform.anchoredPosition = new Vector3(menuOffset.x * -1, menuOffset.y, 0);
        }
        else 
        {
            rectTransform.anchorMax = new Vector2(0,1);
            rectTransform.anchorMin = new Vector2(0,1);
            rectTransform.pivot = new Vector2(0,1);
            rectTransform.anchoredPosition = menuOffset;
        }
    }

    public virtual void Show(Vector3 unitPosition)
    {
        gameObject.SetActive(true);
        MoveMenu(unitPosition);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    
}
