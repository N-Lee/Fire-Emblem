using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : Menu
{
    public GameObject inventoryItemPrefab;
    List<GameObject> inventoryItems;
    Collider2D targetObject;
    Unit character;
    public void Show(Unit character, Collider2D targetObject)
    {
        this.character = character;
        this.targetObject = targetObject;

        gameObject.SetActive(true);
        inventoryItems = new List<GameObject>();
        ShowListOfWeapons(character.inventory.weapons);
    }

    void ShowListOfWeapons(List<Weapon> weaponList)
    {
        foreach (Weapon w in weaponList)
        {
            GameObject newWeaponItem = Instantiate(inventoryItemPrefab, transform);
            Image icon = newWeaponItem.transform.GetChild(0).GetComponent<Image>();
            TMP_Text name = newWeaponItem.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text durability = newWeaponItem.transform.GetChild(2).GetComponent<TMP_Text>();
        
            /* TODO: Show menu
            if (character.IsWeaponUseable(w))
            {
                newWeaponItem.GetComponent<Button>().onClick.AddListener(() => Hide());
            }
            else
            {
                
            }
            */

            // REMOVE WHEN TODO IS DONE ============================================ 
            newWeaponItem.GetComponent<Button>().onClick.AddListener(() => Hide(w));
            // =====================================================================

            icon.sprite = Resources.Load <Sprite>(w.imageLocation);
            name.text = w.name;
            durability.text = w.durabilty.ToString();
        }
    }

    public void Hide(Weapon weapon)
    {
        character.inventory.EquipWeapon(weapon);
        gameObject.SetActive(false);
        gridManager.ShowCombatForecaseMenu(targetObject);
    }
}
