using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Items", menuName = "Inventory/Items", order = 0)]
public class ItemsController : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public float itemValue;
    [System.Serializable]
    public enum TypeItemEnum    
    {
        Weapon,
        Armor,
        Consumable
    }
    [System.Serializable]
    public enum TypeSlotsEnum    
    {
        Inventory,
        Helmet,
        Armor,
        Sword,
        Shield,
        Boots,
        Consumable
    }
    public TypeItemEnum itemType;
    public TypeSlotsEnum slotType;
    
    public void GetAction()
    {
        switch (itemType)
        {
            case TypeItemEnum.Weapon:
                Debug.Log("Weapon");
                break;
            case TypeItemEnum.Armor:
                Debug.Log("Armor");
                break;
            case TypeItemEnum.Consumable:
                Debug.Log("Consumable");
                break;
        }
    }
}
