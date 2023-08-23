using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : Menu
{
    public GameObject inventoryItemPrefab;
    Collider2D targetObject;
    Unit character;
    public void Show(Unit character, Collider2D targetObject)
    {
        this.character = character;
        this.targetObject = targetObject;

        gameObject.SetActive(true);
        ShowListOfWeapons(character.inventory.weapons);
    }

    void ShowListOfWeapons(List<Weapon> weaponList)
    {
        foreach (Weapon w in weaponList)
        {
            GameObject newWeaponItem = Instantiate(inventoryItemPrefab, transform);
            newWeaponItem.GetComponent<InventoryButton>().weapon = w;
            Image icon = newWeaponItem.transform.GetChild(0).GetComponent<Image>();
            TMP_Text name = newWeaponItem.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text durability = newWeaponItem.transform.GetChild(2).GetComponent<TMP_Text>();
        
            icon.sprite = Resources.Load <Sprite>(w.imageLocation);
            name.text = w.name;
            durability.text = w.durabilty.ToString();

            if (character.IsWeaponUseable(w))
            {   
                name.color = new Color(255, 255, 255, 1f);
                durability.color = new Color(255, 255, 255, 1f);
                newWeaponItem.GetComponent<Button>().onClick.AddListener(() => Hide(w));
            }
            else
            {
                name.color = new Color(255, 255, 255, 0.3f);
                durability.color = new Color(255, 255, 255, 0.3f);
            }
        }
    }

    public void Hide(Weapon weapon)
    {
        character.inventory.EquipWeapon(weapon);
        gameObject.SetActive(false);
        GameObject.Find("WeaponDescriptionMenu").GetComponent<WeaponDescriptionMenu>().Hide();
        gridManager.ShowCombatForecaseMenu(targetObject);
    }
}
