using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsVector
{
    public static Vector3 RotateVectorAroundZ(Vector3 directionBase, float rotationValue)
    {
        Vector3 directionRes;

        Vector3 v = directionBase;
        float rotationValueRad = Mathf.PI * 2 * rotationValue / 360;
        float cp = Mathf.Cos(rotationValueRad); float cm = Mathf.Cos(-rotationValueRad);
        float sp = Mathf.Sin(rotationValueRad); float sm = Mathf.Sin(-rotationValueRad);
        List<float> cspm = new List<float>(); cspm.Add(cp); cspm.Add(sp); cspm.Add(cm); cspm.Add(sm);
        //valDbg.x = cp; valDbg.y = cm; valDbg.z = sp; valDbg.w = sm;
        
        directionRes.x = v.x * cm + v.y * sp;
        directionRes.y = v.x * sm + v.y * cp;
        directionRes.z = 0;


        return directionRes;
    }
}

//[ExecuteAlways]
public class VectorTest : MonoBehaviour
{
    public Vector3 directionBase = new Vector3(0, 0.4f, 0.6f);
    public float rotationValue = 0;
    public Vector3 directionRes = new Vector3();

    public bool normalized = false;

    public List<Vector2> addForX = new List<Vector2>();
    public List<Vector2> addForY = new List<Vector2>();
    public Vector4 valDbg;

    public float diff;

    public void Update()
    {
        if (normalized)
        {
            directionBase.Normalize();
            normalized = false;
        }

        Debug.DrawRay(this.transform.position, directionBase, Color.blue, 1 / 60f);
        directionRes = UtilsVector.RotateVectorAroundZ(directionBase, rotationValue);
        Debug.DrawRay(this.transform.position, directionRes, Color.red);

        diff = Vector3.SignedAngle(directionBase, directionRes, Vector3.forward);
    }

}
