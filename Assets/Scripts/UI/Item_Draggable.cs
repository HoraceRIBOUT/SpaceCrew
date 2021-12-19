using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform initialParent = null;
    public RectTransform rect;
    public bool dragOn = false;

    public item itemHere;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("True On begin drag");
        dragOn = true;
        initialParent = this.transform.parent;
        this.transform.SetParent(Vaisseau.instance.ui_man.canvas);//maybe canvas???
        rect = this.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition + (Vector3)rect.rect.size / 2f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragOn = false;
        this.transform.SetParent(initialParent);
        rect.anchoredPosition = Vector3.zero;
    }

}
