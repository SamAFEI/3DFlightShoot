using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUtilities : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 GetDirectionFormAngleInDegrees(float angleInDegree, Vector3 transformForward, Vector3 rotationPlaneAxis)
    {
        float angle = angleInDegree * Mathf.Deg2Rad;
        return (transformForward * Mathf.Cos(angle) + rotationPlaneAxis * Mathf.Sin(angle)).normalized;
    }
}
