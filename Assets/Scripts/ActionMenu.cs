using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    Button attack, item, trade, wait;
    RectTransform rectTransform;
    Vector3 menuOffset = new Vector3(30, -200, 0);
    float leftXOffset = 40f;

    void Update()
    {
        
    }
    
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
}
