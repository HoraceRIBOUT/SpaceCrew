using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Vector3 direction;
    public float damage = 5f;

    public void Update()
    {
        this.transform.position += direction * Time.deltaTime;


        if ((this.transform.position - Vaisseau.instance.mainCam.transform.position).sqrMagnitude > 15f * 15f)
            Death();
            
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
