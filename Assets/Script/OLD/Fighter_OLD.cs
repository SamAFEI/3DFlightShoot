using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_OLD : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public float pitch { get; private set; }
    public float yaw { get; private set; }
    public float thrust { get; private set; }
    public float thrust_multiplier { get; private set; }
    public float yaw_multiplier { get; private set; }
    public float pitch_multiplier { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        thrust = 1000;
        thrust_multiplier = 37;
        yaw_multiplier = 160;
        pitch_multiplier = 160;
    }

    private void FixedUpdate()
    {
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Horizontal");

        rb.AddRelativeForce(0f, 0f, thrust * thrust_multiplier * Time.deltaTime);
        rb.AddRelativeTorque(pitch * pitch_multiplier * Time.deltaTime, 
            yaw * yaw_multiplier * Time.deltaTime,
            -yaw * yaw_multiplier * 2 * Time.deltaTime);
    }
}
