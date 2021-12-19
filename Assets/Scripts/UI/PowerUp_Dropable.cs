using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUp_Dropable : MonoBehaviour, IDropHandler
{
    public Character chara;
    public item currentItem;

    [Header("Gray")]
    public UnityEngine.UI.Image graySprite;
    public Color lockColor = Color.gray;
    private Color targetColor = Color.clear;
    public float graySpeed = 4f;

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
                    it.convForPilot_try.actionToPerformAtEnd += FinishTry;
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForPilot_try);
                    break;
                case Character.milit:
                    it.convForMilit_try.actionToPerformAtEnd += FinishTry;
                    Vaisseau.instance.ui_man.conversation.StartThisConversation(it.convForMilit_try);
                    break;
                case Character.mecan:
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
        Debug.Log("Finish try !!!");
        Vaisseau.instance.ui_man.inventory.pwpUp = this;
        Vaisseau.instance.ui_man.inventory.Validation(true);
    }

    public void Conf()
    {
        Debug.Log("Conf is made !");
        switch (chara)
        {
            case Character.None:
                break;
            case Character.pilot:
                currentItem.convForPilot_try.actionToPerformAtEnd += FinishConf;
                Vaisseau.instance.ui_man.conversation.StartThisConversation(currentItem.convForPilot_conf);
                break;
            case Character.milit:
                currentItem.convForMilit_try.actionToPerformAtEnd += FinishConf;
                Vaisseau.instance.ui_man.conversation.StartThisConversation(currentItem.convForMilit_conf);
                break;
            case Character.mecan:
                currentItem.convForMecan_try.actionToPerformAtEnd += FinishConf;
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

        Debug.Log("Finish confffff !!!");

        Vaisseau.instance.ApplyItem(currentItem, chara);
        Vaisseau.instance.RemInInv(currentItem);

        currentItem = null;


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
