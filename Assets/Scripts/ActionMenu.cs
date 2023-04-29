using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    [SerializeField] Button attack, staff, rescue, item, trade, wait;
    [SerializeField] GridManager gridManager;
    public bool phaseEnd;
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

    public void Show(List<string> options)
    {
        gameObject.SetActive(true);
        SelectOptions(options);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        staff.gameObject.SetActive(false);
        rescue.gameObject.SetActive(false);
        item.gameObject.SetActive(false);
        trade.gameObject.SetActive(false);
        wait.gameObject.SetActive(false);
    }

    void SelectOptions(List<string> options)
    {
        foreach (string s in options)
        {
            switch(s)
            {
                case "Attack":
                    attack.gameObject.SetActive(true);
                    break;
                
                case "Staff":
                    staff.gameObject.SetActive(true);
                    break;
                
                case "Rescue":
                    rescue.gameObject.SetActive(true);
                    break;

                case "Item":
                    item.gameObject.SetActive(true);
                    break;

                case "Trade":
                    trade.gameObject.SetActive(true);
                    break;
                
                case "Wait":
                    wait.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
