using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUp_Dropable : MonoBehaviour, IDropHandler
{
    public Character chara;
    public Item_Draggable drag;
    public item currentItem;

    [Header("Gray")]
    public UnityEngine.UI.Image graySprite;
    public Color lockColor = Color.gray;
    private Color targetColor = Color.clear;
    public float graySpeed = 4f;

    private Conversation lastConv = null;

    public void OnDrop(PointerEventData eventData)
    {
     //   if ()
        {
            drag = eventData.pointerDrag.GetComponent<Item_Draggable>();
            Debug.Log("Received : " + (drag != null));
            item it = drag.itemHere;
            Debug.Log("Get : "+it.type);
            currentItem = it;
            switch (chara)
            {
                case Character.None:
                    break;
                case Character.pilot:
                    lastConv = it.convForPilot_try;
                    it.convForPilot_try.actionToPerformAtEnd += FinishTry;
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForPilot_try);
                    break;
                case Character.milit:
                    lastConv = it.convForMilit_try;
                    it.convForMilit_try.actionToPerformAtEnd += FinishTry;
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForMilit_try);
                    break;
                case Character.mecan:
                    lastConv = it.convForMecan_try;
                    it.convForMecan_try.actionToPerformAtEnd += FinishTry;
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

    public void FinishTry()
    {
        lastConv.actionToPerformAtEnd =null;
        Debug.Log("Finish try !!!");
        Vaisseau.instance.ui_man.inventory.pwpUp = this;
        Vaisseau.instance.ui_man.inventory.Validation(true);
    }

    public void Refuse()
    {
        Debug.Log("Refuse after try.");
        drag = null;
        currentItem = null;
        Vaisseau.instance.ui_man.inventory.ChangePwpUp_Default();
    }

    public void Conf()
    {
        Debug.Log("Conf is made !");
        switch (chara)
        {
            case Character.None:
                break;
            case Character.pilot:
                lastConv = currentItem.convForPilot_conf;
                currentItem.convForPilot_conf.actionToPerformAtEnd += FinishConf;
                Vaisseau.instance.ui_man.conversation.StartThisConversation(currentItem.convForPilot_conf);
                break;
            case Character.milit:
                lastConv = currentItem.convForMilit_conf;
                currentItem.convForMilit_conf.actionToPerformAtEnd += FinishConf;
                Vaisseau.instance.ui_man.conversation.StartThisConversation(currentItem.convForMilit_conf);
                break;
            case Character.mecan:
                lastConv = currentItem.convForMecan_conf;
                currentItem.convForMecan_conf.actionToPerformAtEnd += FinishConf;
                Vaisseau.instance.ui_man.conversation.StartThisConversation(currentItem.convForMecan_conf);
                break;
            case Character.alien:
                Debug.LogError("How did you get here ?");

                break;
            default:
                break;
        }
    }

    public void FinishConf()
    {
        lastConv.actionToPerformAtEnd = null;
        Debug.Log("Finish confffff !!!");

        Vaisseau.instance.ApplyItem(currentItem, chara);
        Vaisseau.instance.RemInInv(currentItem);

        drag.EmptyIt();
        drag = null;
        currentItem = null;
        Vaisseau.instance.ui_man.inventory.UpdateInvVisual();
        Vaisseau.instance.ui_man.inventory.ChangePwpUp_Default();
    }
    
    public void Update()
    {
        if(graySprite.color != targetColor)
        {
            graySprite.color = Color.Lerp(graySprite.color, targetColor, Time.deltaTime * graySpeed);

            if(Mathf.Abs((graySprite.color - targetColor).maxColorComponent) < 0.04f)
            {
                graySprite.color = targetColor;
            }
        }
    }

    public void SetGray(bool gray)
    {
        targetColor = gray ? lockColor : Color.clear;
    }


}
