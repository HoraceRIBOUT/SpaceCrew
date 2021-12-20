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
    public float pvMax;
    public float armor;
    public float surchauffeMax;

    public float regen          ;
    public float contactDamage  ;
    public float shotSize       ;

    public static Stat operator +(Stat a ,Stat b)
    {
        a.speedMax += b.speedMax;
        a.speedGain += b.speedGain;
        a.friction += b.friction;
        a.damage += b.damage;
        a.pv += b.pv;
        a.pvMax += b.pvMax;
        a.armor += b.armor;
        a.surchauffeMax += b.surchauffeMax;
        a.regen += b.regen;
        a.contactDamage += b.contactDamage;
        a.shotSize += b.shotSize;
        return a;
    }

    public static Stat Lerp(Stat a, Stat b, float lerp)
    {
        a.speedMax      = Mathf.Lerp(a.speedMax     , b.speedMax        ,lerp);
        a.speedGain     = Mathf.Lerp(a.speedGain    , b.speedGain       ,lerp);
        a.friction      = Mathf.Lerp(a.friction     , b.friction        ,lerp);   
        a.damage        = Mathf.Lerp(a.damage       , b.damage          ,lerp);
        a.pv            = Mathf.Lerp(a.pv           , b.pv              ,lerp);
        a.pvMax         = Mathf.Lerp(a.pvMax        , b.pvMax           ,lerp);
        a.armor         = Mathf.Lerp(a.armor        , b.armor           ,lerp);
        a.surchauffeMax = Mathf.Lerp(a.surchauffeMax, b.surchauffeMax   ,lerp);
        a.regen         = Mathf.Lerp(a.regen        , b.regen           , lerp);
        a.contactDamage = Mathf.Lerp(a.contactDamage, b.contactDamage   , lerp);
        a.shotSize      = Mathf.Lerp(a.shotSize     , b.shotSize        , lerp);
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

    [Header("Continuous movement")]
    public Vector3 movementRange = new Vector3();
    public Vector3 movement = Vector3.zero;

    public Vector2 rotSpeed = new Vector2(0,12);
    public float rotSpeed_const = 0;

    public GameObject destroyFX;

    public void Start()
    {
        sR.sprite = itemCollect.forSpace;

        if (movement == Vector3.zero)
            movement = new Vector3(Random.Range(-movementRange.x, movementRange.x), Random.Range(-movementRange.y, movementRange.y), 0);
        this.transform.Rotate(Vector3.forward * Random.Range(-180, 180));
        rotSpeed_const = (Random.Range(0, 100) > 50 ? 1 : -1) * Random.Range(rotSpeed.x, rotSpeed.y);
    }

    public void Update()
    {
        this.transform.position += movement * Time.deltaTime;
        this.transform.Rotate(Vector3.forward * rotSpeed_const * Time.deltaTime);
    }


    public void Death()
    {
        if (destroyFX != null)
        {
            Instantiate(destroyFX, this.transform.position, this.transform.rotation);
        }

        Destroy(this.gameObject);
    }
}
