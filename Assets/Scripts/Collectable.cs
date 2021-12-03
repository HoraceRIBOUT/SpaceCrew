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
//Probably also a item list
public enum ItemCollectable
{
    None,
    copperOre,
    ironOre,
    cocktail,
    teddyBear,
    cdPlayer,
    demonHeart,
    demonLazerEye,
    turret,
    motor,

}


public class Collectable : MonoBehaviour
{
    public ItemCollectable itemType = ItemCollectable.None;
    //[SerializeField]
    public Stat statToAdd;


    public void Death()
    {
        Destroy(this.gameObject);
    }
}
