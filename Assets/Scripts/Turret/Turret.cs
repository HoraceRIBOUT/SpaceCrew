using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform startPoint;

    public Vector3 offsetDirection = Vector3.zero;

    public Transform canonVisual;
    public float shootSpeed = 3f;


    public float cooldown;
    public GameObject prefabShoot;

    private float lastShot = 0;

    public virtual void Try_Shoot(Vector3 targetPos, float dmgMultiplier = 1)
    {

        if (Time.time - lastShot > cooldown)
        {
            Shoot(targetPos, dmgMultiplier);
        }
    }

    protected virtual void Shoot(Vector3 targetPos, float dmgMultiplier = 1)
    {
        GameObject shootGO = Instantiate(prefabShoot, startPoint.position, Quaternion.identity, null);
        Shoot shootShoot = shootGO.GetComponent<Shoot>();
        if(shootShoot != null)
        {
            shootShoot.direction = targetPos - startPoint.transform.position;
            shootShoot.direction.Normalize();
            shootShoot.direction *= shootSpeed;
            shootShoot.damage *= dmgMultiplier;
        }
        lastShot = Time.time;
    }

    public virtual void UpdateVisual(Vector3 targetPos)
    {
        Vector3 currentWorldForwardDirection = transform.TransformDirection(Vector3.up);
        Vector3 direction = targetPos - startPoint.transform.position;
        float angleDiff = Vector3.SignedAngle(currentWorldForwardDirection, direction.normalized, Vector3.forward);


        canonVisual.rotation = Quaternion.Euler(Vector3.forward * angleDiff + offsetDirection);
    }
}
