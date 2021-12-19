using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectable", menuName = "CustomScriptableObject/Collectable", order = 1)]
public class item : ScriptableObject
{
    public ItemCollectable type;

    //
    public bool giveMilit = false;
    public Conversation convForMilit_try;
    public Conversation convForMilit_conf;
    public Stat statToAddMilit;
    public Rom_Stat romStatToAddToMilit;
    public bool givePilot = false;
    public Conversation convForPilot_try;
    public Conversation convForPilot_conf;
    public Stat statToAddPilot;
    public Rom_Stat romStatToAddToPilot;
    public bool giveMecan = false;
    public Conversation convForMecan_try;
    public Conversation convForMecan_conf;
    public Stat statToAddMecan;
    public Rom_Stat romStatToAddToMecan;


    public Color color = Color.white;
    public Sprite forSpace;
    public Sprite forInv;

    [Header("Special case")]
    public Conversation onPickUp;
}
