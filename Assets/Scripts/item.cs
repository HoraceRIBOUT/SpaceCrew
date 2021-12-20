using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Collectable", menuName = "CustomScriptableObject/Collectable", order = 1)]
public class item : ScriptableObject
{
    public ItemCollectable type;
    [Tooltip("Put to 1 to make it unique. -1 for infinite.")]
    public int numberPossible = -1;

    //
    [Header("Milit")]
    [FoldoutGroup("Milit")]public bool giveMilit = false;
    [FoldoutGroup("Milit")]public Conversation convForMilit_try;
    [FoldoutGroup("Milit")]public Conversation convForMilit_conf;
    [FoldoutGroup("Milit")]public Stat statToAddMilit;
    [FoldoutGroup("Milit")]public Rom_Stat romStatToAddToMilit;
    [Header("Pilot")]
    [FoldoutGroup("Pilot")]public bool givePilot = false;
    [FoldoutGroup("Pilot")]public Conversation convForPilot_try;
    [FoldoutGroup("Pilot")]public Conversation convForPilot_conf;
    [FoldoutGroup("Pilot")]public Stat statToAddPilot;
    [FoldoutGroup("Pilot")] public Rom_Stat romStatToAddToPilot;
    [Header("Mecan")]
    [FoldoutGroup("Mecan")]public bool giveMecan = false;
    [FoldoutGroup("Mecan")]public Conversation convForMecan_try;
    [FoldoutGroup("Mecan")]public Conversation convForMecan_conf;
    [FoldoutGroup("Mecan")]public Stat statToAddMecan;
    [FoldoutGroup("Mecan")] public Rom_Stat romStatToAddToMecan;


    public Color color = Color.white;
    public Sprite forSpace;
    public Sprite forInv;

    [Header("Special case")]
    public Conversation onPickUp;

    public Turret addTuret = null;

}
