using UnityEngine;

public static class HelperUtilities
{
    public static Vector3 GetDirectionFormAngleInDegrees(float angleInDegree, Vector3 transformForward, Vector3 rotationPlaneAxis)
    {
        float angle = angleInDegree * Mathf.Deg2Rad;
        return (transformForward * Mathf.Cos(angle) + rotationPlaneAxis * Mathf.Sin(angle)).normalized;
    }
}
