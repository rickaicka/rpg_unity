using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject SlotParent;
    public List<SlotsController> SlotList = new List<SlotsController>();
    public static InventoryController Instance;
    private void Start()
    {
        Instance = this;
        GetSlots();
    }

    public void GetSlots()
    {
        foreach (SlotsController slot in SlotParent.GetComponentsInChildren<SlotsController>())
        {
            SlotList.Add(slot);
        }
        //CreateItem();
    }

    public void CreateItem(ItemsController item)
    {
        foreach (SlotsController slot in SlotList)
        {
            if(slot.transform.childCount == 0)
            {
                GameObject CurrentItem = Instantiate(GameController.Instance.ItemPrefab, slot.transform);
                CurrentItem.GetComponent<DragIItemController>().item = item;
                
                return;
            }
        }
    }
}
