using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotsController : MonoBehaviour, IDropHandler
{
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
    public TypeSlotsEnum slotType;
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragIItemController.ItemBeginDrag.GetComponent<DragIItemController>().SetParent(transform, this);
        }
    }
}
