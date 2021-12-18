using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    public float speedMax;
    public float speedGain;
    public float friction;
    public float damage;
    public float pv;
    public float armor;


}
[System.Serializable]
public struct Rom_Stat
{
    public float affection;
    public float love;
    public float openess;

    public static Rom_Stat operator +(Rom_Stat a, Rom_Stat b)
    {
        a.affection += b.affection;
        a.love += b.love;
        a.openess += b.openess;
        return a;
    }
}
//Probably also a item list
public enum ItemCollectable
{
    None,
    cdPlayer,
    demonHeart,
    demonHeartFantom,
    demonHeartDry,
    copperOre,
    ironOre,
    waterBottle,
    fireCrystal,
    metalScrap,
    motor,
    pistol,
    potion,
    uranium,
    untreatedGold,
    teddyBear, 
    sword,
    turret,
    demonLazerEye,

}

[CreateAssetMenu(fileName = "Collectable", menuName = "CustomScriptableObject/Collectable", order = 1)]
public class item : ScriptableObject
{
    public ItemCollectable type;
    public Stat statToAdd;
    public Rom_Stat romStatToAdd;

    //
    public bool giveMilit = false;
    public Conversation convForMilit_try;
    public Conversation convForMilit_conf;
    public bool givePilot = false;
    public Conversation convForPilot_try;
    public Conversation convForPilot_conf;
    public bool giveMecan = false;
    public Conversation convForMecan_try;
    public Conversation convForMecan_conf;

    

    public Sprite forSpace;
    public Sprite forInv;

    [Header("Special case")]
    public Conversation onPickUp;
}

public class Collectable : MonoBehaviour
{
    public item itemCollect;
    public SpriteRenderer sR;

    public void Start()
    {
        sR.sprite = itemCollect.forSpace;
    }


    public void Death()
    {
        Destroy(this.gameObject);
    }
}
