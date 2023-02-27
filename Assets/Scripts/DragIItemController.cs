using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeginDrag;

    private Vector3 startPosition;
    private Transform startParent;
    public ItemsController item;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = item.itemImage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeginDrag = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeginDrag = null; 
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }

    public void SetParent(Transform slotTransform, SlotsController slot)
    {
        if ((int)slot.slotType == (int)item.slotType)
        {
            transform.SetParent(slotTransform);
            item.GetAction();
        }else if (slot.slotType == SlotsController.TypeSlotsEnum.Inventory)
        {
            transform.SetParent(slotTransform);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
