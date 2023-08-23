using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDescriptionMenu : MonoBehaviour
{
    Unit character;
    RectTransform rectTransform;
    string weaponImagePath;

    public void MoveMenu(bool isRightSide)
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (isRightSide)
        {
            rectTransform.anchorMax = new Vector2(1,0);
            rectTransform.anchorMin = new Vector2(1,0);
            rectTransform.pivot = new Vector2(1,0);
            rectTransform.anchoredPosition = new Vector3(-100, 100, 0);
        }
        else
        {
            rectTransform.anchorMax = new Vector2(0,0);
            rectTransform.anchorMin = new Vector2(0,0);
            rectTransform.pivot = new Vector2(0,0);
            rectTransform.anchoredPosition = new Vector3(100, 100, 0);
        }
    }

    void UpdateDetails(Weapon weapon)
    {
        if (!character)
        {
            character = GameObject.Find("GridManager").GetComponent<GridManager>().selectedCharacter;
        }

        Transform[] transforms = GameObject.Find("WeaponDescriptionMenu").GetComponentsInChildren<Transform>(true);
        foreach(Transform t in transforms)
        {
            if (t.name == "AffiImage")
            {
                UpdateWeaponType(weapon);
                Image image = t.gameObject.GetComponent<Image>();
                Sprite sprite = Resources.Load <Sprite>(weaponImagePath);
                image.sprite = sprite;
            }
            else if (t.name == "AtkValue")
            {
                TMP_Text text = t.gameObject.GetComponent<TMP_Text>();
                text.text = weapon.might.ToString();
            }
            else if (t.name == "CritValue")
            {
                TMP_Text text = t.gameObject.GetComponent<TMP_Text>();
                text.text = weapon.critRate.ToString();
            }
            else if (t.name == "HitValue")
            {
                TMP_Text text = t.gameObject.GetComponent<TMP_Text>();
                text.text = weapon.hitRate.ToString();
            }
            else if (t.name == "AvoidValue")
            {
                TMP_Text text = t.gameObject.GetComponent<TMP_Text>();
                text.text = character.AvoidRate().ToString();
            }
        }
    }

    void UpdateWeaponType(Weapon weapon)
    {
        switch (weapon.weaponType)
        {
            case WeaponType.sword:
            {
                weaponImagePath = "Image/Weapons/SilverSword";
                break;
            }
            case WeaponType.axe:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.lance:
            {
                // TODO: Update image paths
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.staff:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.wind:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.fire:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.thunder:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.light:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.dark:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
            case WeaponType.anima:
            {
                weaponImagePath = "Image/Weapons/ShortAxe";
                break;
            }
        
        }
    }

    public void Show(bool isRightSide, Weapon weapon)
    {
        gameObject.SetActive(true);
        MoveMenu(isRightSide);
        UpdateDetails(weapon);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
