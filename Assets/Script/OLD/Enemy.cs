using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public Transform model { get; private set; }
    public Transform leftAvoider { get; private set; }
    public Transform rightAvoider { get; private set; }
    public float avoideDistance { get; private set; }
    public float torque = 500f;
    public float thrust = 1000f;

    public LayerMask RayLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        model = transform.Find("Model");
        leftAvoider = transform.Find("LeftAvoider").transform;
        rightAvoider = transform.Find("RightAvoider").transform;
        avoideDistance = 100f;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        Vector3 vector = transform.position - collision.collider.ClosestPoint(transform.position);
        vector = vector.normalized;
        rb.AddForce(vector * 1000f, ForceMode.Impulse);
    }

    private void Movement()
    {
        Vector3 targetDir = GameManager.GetPlayerDirection(transform.position);
        Vector3 playerPos = GameManager.Instance.playerPos;

        float xyAngle = GetAngleOnPlane(playerPos, transform.position, transform.forward, transform.up);
        float yzAngle = GetAngleOnPlane(playerPos, transform.position, transform.right, transform.forward);
        if (Mathf.Abs(xyAngle) >= 1f && Mathf.Abs(yzAngle) >= 1f)
        {
            rb.AddRelativeTorque(Vector3.forward * -torque * (xyAngle / Mathf.Abs(xyAngle)));
        }
        else if (yzAngle >= 1f)
        {
            rb.AddRelativeTorque(Vector3.right * -torque);

            //fire();
        }
        rb.AddRelativeForce(Vector3.forward * thrust);
    }

    private float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toOrientation)
    {
        Vector3 vector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float angle = Vector3.SignedAngle(vector, toOrientation, planeNormal);
        return angle;
    }
}
