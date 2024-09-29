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
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        AvoidRay();
        float xyAngle = GetAngleOnPlane(playerPos, transform.position, transform.forward, transform.up);
        float yzAngle = GetAngleOnPlane(playerPos, transform.position, transform.right, transform.forward);
        AvoidObstacles();
        if (true)
        {
            Debug.Log("forward");
            if (Mathf.Abs(xyAngle) >= 1f && Mathf.Abs(yzAngle) >= 1f)
            {
                rb.AddRelativeTorque(Vector3.forward * -torque * (xyAngle / Mathf.Abs(xyAngle)));
            }
            else if (yzAngle >= 1f)
            {
                rb.AddRelativeTorque(Vector3.right * -torque);

                //fire();
            }

        }
        

        rb.AddRelativeForce(Vector3.forward * thrust);
    }

    private float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toOrientation)
    {
        Vector3 vector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float angle = Vector3.SignedAngle(vector, toOrientation, planeNormal);
        return angle;
    }


    private void AvoidRay()
    {
        List<Vector3> avoiderDir = new List<Vector3>()
        {
            model.transform.forward,
            (model.transform.forward + model.transform.right).normalized,
            (model.transform.forward - model.transform.right).normalized
        };
        foreach (var vector in avoiderDir)
        {
            Debug.DrawRay(transform.position, vector * avoideDistance, Color.blue);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, avoiderDir[0], out hit, avoideDistance, RayLayer))
        {
            for (int i = 1; i < avoiderDir.Count; i++)
            {
                bool canTurn = CheckTurn(avoiderDir[i]);
                if (canTurn)
                {
                    break;
                }
            }
        }
    }

    private bool CheckTurn(Vector3 dir)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, dir, out hit, avoideDistance / 4f, RayLayer))
        {
            return true;
        }
        return false;
    }

    private bool AvoidObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100, RayLayer) ||
            Physics.Raycast(leftAvoider.position, leftAvoider.forward, out hit, 100, RayLayer) ||
            Physics.Raycast(rightAvoider.position, rightAvoider.forward, out hit, 100, RayLayer))
        {
            float xyAngle = GetAngleOnPlane(hit.point, transform.position, transform.forward, transform.up); 
            float yzAngle = GetAngleOnPlane(hit.point, transform.position, transform.right, transform.forward);

            if (Mathf.Abs(xyAngle) >= 1f && Mathf.Abs(yzAngle) >= 1f)
            {
                rb.AddRelativeTorque(Vector3.forward * -torque * (xyAngle / Mathf.Abs(xyAngle)));
            }
            else if (yzAngle >= 1f)
            {
                rb.AddRelativeTorque(Vector3.right * -torque);
            }
            Debug.Log("aviod");
            return true;
        }
        return false;
    }

}
