using System;
using UnityEngine;

public class Flight05 : MonoBehaviour
{
    public Transform Model { get; private set; }
    public Rigidbody RB { get; private set; }

    public float Pitch;
    public float Yaw;
    public float Roll;
    public float Strafe;
    public float Throttle;
    public Vector3 moveInput;
    public Vector3 mouseInput, screenCenter;

    public Vector3 linearForce = new Vector3(100.0f, 100.0f, 100.0f);
    public Vector3 angularForce = new Vector3(1.0f, 1.0f, 1.0f);

    [Range(0.0f, 1.0f)]
    public float reverseMultiplier = 1.0f;
    public float forceMultiplier = 100.0f;

    private const float THROTTLE_SPEED = 0.5f;
    private Vector3 appliedLinearForce = Vector3.zero;
    private Vector3 appliedAngularForce = Vector3.zero;

    private void Awake()
    {
        Model = transform.Find("Model");
        RB = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

    }

    private void Update()
    {
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Roll");
        mouseInput = Input.mousePosition;
        mouseInput.z = 1000f;
        Vector3 gotoPos = Camera.main.ScreenToWorldPoint(mouseInput); 
        TurnTowardsPoint(gotoPos); 
        BankShipRelativeToUpVector(mouseInput, Vector3.up);
        SetPhysics(new Vector3(Strafe, 0.0f, Throttle), new Vector3(Pitch, Yaw, Roll)); 
        //SetPhysics(new Vector3(Strafe, 0.0f, Throttle), new Vector3(0, 0, 0));
    }

    private void FixedUpdate()
    {
        UpdateThrottle();
        if (RB != null)
        {
            RB.AddRelativeForce(appliedLinearForce * forceMultiplier, ForceMode.Force);
            RB.AddRelativeTorque(appliedAngularForce * 1f, ForceMode.Force);
        }
    }

    #region Input
    private void UpdateThrottle()
    {
        float target = Throttle;

        if (moveInput.y > 0) { target = 1.0f; }
        else { target = 0.0f; }

        Throttle = Mathf.MoveTowards(Throttle, target, Time.deltaTime * THROTTLE_SPEED);
    }
    private void TurnTowardsPoint(Vector3 gotoPos)
    {
        Vector3 localGotoPos = transform.InverseTransformVector(gotoPos - transform.position).normalized;

        // Note that you would want to use a PID controller for this to make it more responsive.
        Pitch = Mathf.Clamp(-localGotoPos.y * 2.5f, -1f, 1f);
        Yaw = Mathf.Clamp(localGotoPos.x * 2.5f, -1f, 1f);
    }
    private void BankShipRelativeToUpVector(Vector3 mousePos, Vector3 upVector)
    {
        float bankInfluence = (mousePos.x - screenCenter.x) / (screenCenter.x);
        bankInfluence = Mathf.Clamp(bankInfluence, -1f, 1f);

        // Throttle modifies the bank angle so that when at idle, the ship just flatly yaws.
        bankInfluence *= Throttle;
        float bankTarget = bankInfluence * 35f;

        // Here's the special sauce. Roll so that the bank target is reached relative to the
        // up of the camera.
        float bankError = Vector3.SignedAngle(transform.up, upVector, transform.forward);
        bankError = bankError - bankTarget;

        // Clamp this to prevent wild inputs.
        bankError = Mathf.Clamp(bankError * 0.1f, -1f, 1f);

        // Roll to minimze error.
        Roll = bankError * 1f;
    }
    #endregion

    #region Physics
    public void SetPhysics(Vector3 linear, Vector3 angular)
    {
        appliedLinearForce = Vector3.Scale(linearForce, linear);
        appliedAngularForce = Vector3.Scale(angularForce, angular);
    }
    #endregion
}

