using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float pv = 1;
    public List<GameObject> dropableObject;
    public int numberItemDrop = 0;
    public bool notDouble = false;
    public float distanceDrop = 1f;

    public Vector3 movementRange = new Vector3();
    public Vector3 movement = Vector3.zero;

    public float rotSpeed = 0;

    public GameObject destroyFX;

    public void Start()
    {
        if(movement == Vector3.zero)
            movement = new Vector3(Random.Range(-movementRange.x, movementRange.x), Random.Range(-movementRange.y, movementRange.y), 0);
        this.transform.Rotate(Vector3.forward * Random.Range(-180, 180));
    }

    public void Update()
    {
        this.transform.position += movement * Time.deltaTime;
        this.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }



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
                Vector3 randomOffset = new Vector3( Random.Range(-1, 1),Random.Range(-1,1), 0);
                randomOffset = randomOffset.normalized * Random.Range(0, distanceDrop);

                int randomIndex = Random.Range(0, dropableObject.Count);
                Collectable collec = Instantiate(dropableObject[randomIndex], this.transform.position + randomOffset, this.transform.rotation).GetComponent<Collectable>();
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
