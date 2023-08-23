using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    WeaponDescriptionMenu weaponMenu;
    InventoryMenu inventoryMenu;
    public Weapon weapon;

    void Start()
    {
        Transform[] canvas = GameObject.Find("UI Canvas").GetComponentsInChildren<Transform>(true);
        foreach(Transform t in canvas)
        {
            if (t.name == "WeaponDescriptionMenu")
            {
                weaponMenu = t.gameObject.GetComponent<WeaponDescriptionMenu>();
            }
            else if (t.name == "InventoryMenu")
            {
                inventoryMenu = t.gameObject.GetComponent<InventoryMenu>();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        weaponMenu.Show(inventoryMenu.IsRightSide(), weapon);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        weaponMenu.Hide();
    }
}
