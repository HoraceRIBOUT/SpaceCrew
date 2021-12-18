using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float pv = 1;
    public List<GameObject> dropableObject;
    public int numberItemDrop = 0;
    public bool notDouble = false;

    public GameObject destroyFX;

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
        if(dropableObject.Count > 0)
        {

            for (int i = 0; i < numberItemDrop && dropableObject.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, dropableObject.Count);
                Collectable collec = Instantiate(dropableObject[randomIndex], this.transform.position, this.transform.rotation).GetComponent<Collectable>();
                //Push it in a random direction with rotation

                if (notDouble)
                {
                    dropableObject.RemoveAt(randomIndex);
                }
            }
        }

        if(destroyFX != null)
        {
            Instantiate(destroyFX, this.transform.position, this.transform.rotation);
        }

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
