using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUp_Dropable : MonoBehaviour, IDropHandler
{
    public Character chara;
    public item currentItem;

    public void OnDrop(PointerEventData eventData)
    {
     //   if ()
        {
            Debug.Log("Received : " + (eventData.pointerDrag.GetComponent<Item_Draggable>() != null));
            item it = eventData.pointerDrag.GetComponent<Item_Draggable>().itemHere;
            Debug.Log("Get : "+it.type);
            currentItem = it;
            switch (chara)
            {
                case Character.None:
                    break;
                case Character.pilot:
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForPilot_try);
                    break;
                case Character.milit:
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForMilit_try);
                    break;
                case Character.mecan:
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForMecan_try);
                    break;
                case Character.alien:
                    Debug.LogError("How did you get here ?");

                    break;
                default:
                    break;
            }
        }
    }
}
