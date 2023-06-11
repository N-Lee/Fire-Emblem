using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : Menu
{
    [SerializeField] Button attack, staff, rescue, item, trade, wait;
    public bool phaseEnd;

    public void Show(Vector3 unitPosition, List<string> options)
    {
        gameObject.SetActive(true);
        SelectOptions(options);
        MoveMenu(unitPosition);
    }

    public override void Hide()
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
