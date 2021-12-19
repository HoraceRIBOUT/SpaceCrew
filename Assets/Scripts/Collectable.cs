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
    public float surchauffeMax;

    public static Stat operator +(Stat a ,Stat b)
    {
        a.speedMax += b.speedMax;
        a.speedGain += b.speedGain;
        a.friction += b.friction;
        a.damage += b.damage;
        a.pv += b.pv;
        a.armor += b.armor;
        a.surchauffeMax += b.surchauffeMax;
        return a;
    }
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
    public static Rom_Stat operator -(Rom_Stat a, Rom_Stat b)
    {
        a.affection -= b.affection;
        a.love -= b.love;
        a.openess -= b.openess;
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
    fantomOre,
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
