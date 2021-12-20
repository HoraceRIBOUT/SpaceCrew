using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform initialParent = null;
    public UnityEngine.UI.Image sR;
    public RectTransform rect;
    public bool dragOn = false;

    public item itemHere;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemHere == null)
            return;

        Debug.Log("True On begin drag");
        dragOn = true;
        initialParent = this.transform.parent;
        this.transform.SetParent(Vaisseau.instance.ui_man.canvas);//maybe canvas???
        rect = this.GetComponent<RectTransform>();
        sR.raycastTarget = false;

        Vaisseau.instance.ui_man.inventory.ChangePwpUp(itemHere);
    }

    public void Update()
    {
        if (dragOn)
        {
            Vector3 midSize = rect.rect.size / 2f;
            midSize.x = -midSize.x;
            this.transform.position = Input.mousePosition + midSize;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragOn = false;
        this.transform.SetParent(initialParent);
        rect.anchoredPosition = Vector3.zero;
        sR.raycastTarget = true;
        Vaisseau.instance.ui_man.inventory.ChangePwpUp_Default();
        
    }

    public void EmptyIt()
    {
        sR.raycastTarget = false;
        sR.sprite = null;
        itemHere = null;
    }

}
