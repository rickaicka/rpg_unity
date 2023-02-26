using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Items", menuName = "Inventory/Items", order = 0)]
public class ItemsController : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public float itemValue;
    [System.Serializable]
    public enum TypeItem    
    {
        Potion,
        Weapon,
        Armor,
    }
    public TypeItem itemType;
    
    public void GetItem()
    {
        switch (itemType)
        {
            case TypeItem.Potion:
                Debug.Log("Potion");
                break;
            case TypeItem.Weapon:
                Debug.Log("Weapon");
                break;
            case TypeItem.Armor:
                Debug.Log("Armor");
                break;
        }
    }
}
