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
    public float chauffe = 0.1f;
    public GameObject prefabShoot;

    private float lastShot = 0;

    public virtual float Try_Shoot(Vector3 targetPos, float dmgMultiplier = 1)
    {
        if (Time.time - lastShot > cooldown)
        {
            Shoot(targetPos, dmgMultiplier);
            return chauffe;
        }
        return 0;
    }

    protected virtual void Shoot(Vector3 targetPos, float dmgMultiplier = 1)
    {
        GameObject shootGO = Instantiate(prefabShoot, startPoint.position, Quaternion.identity, null);
        Shoot shootShoot = shootGO.GetComponent<Shoot>();
        if(shootShoot != null)
        {
            Vector3 directionShoot = (targetPos - startPoint.transform.position).normalized;
            
            shootShoot.direction = UtilsVector.RotateVectorAroundZ(directionShoot, offsetDirection.z);

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


        canonVisual.localRotation = Quaternion.Euler(Vector3.forward * angleDiff - offsetDirection);
    }
}
