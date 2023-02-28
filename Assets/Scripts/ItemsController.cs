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
    
    public PlayerController player;
    
    public void GetAction()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        switch (itemType)
        {
            case TypeItemEnum.Weapon:
                player.IncreaseStats(0, itemValue);
                break;
            case TypeItemEnum.Armor:
                player.IncreaseStats(itemValue, itemValue / .25f);
                break;
            case TypeItemEnum.Consumable:
                player.IncreaseStats(itemValue, 0f);
                break;
        }
    }
    public void RemoveAction()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        switch (itemType)
        {
            case TypeItemEnum.Weapon:
                player.DecreaseStats(0, itemValue);
                break;
            case TypeItemEnum.Armor:
                player.DecreaseStats(itemValue, itemValue / .25f);
                break;
            case TypeItemEnum.Consumable:
                player.DecreaseStats(itemValue, 0f);
                break;
        }
    }
}
