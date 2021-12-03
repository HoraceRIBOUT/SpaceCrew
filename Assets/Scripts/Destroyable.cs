using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float pv = 1;

    public void GetHit(float damage = 1)
    {
        pv -= damage;

        if(pv <= 0)
        {
            Death();
        }
    }


    public void Death()
    {
        Destroy(this.gameObject);
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        Shoot shoot = collision.gameObject.GetComponent<Shoot>();
        if (shoot != null)
        {
            GetHit(shoot.damage);
            //Probably spawn some particle
            shoot.Death();
        }
    }

}
