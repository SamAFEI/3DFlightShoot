using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight02 : MonoBehaviour
{
    public float forwardInput;
    public float forwardSpeed = 25f;
    public float strafeSpeed = 7.5f;
    public float hoverSpeed = 5f;
    public Vector2 moveInput;

    public float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    public float forwardAcc = 2.5f;
    public float strafeAcc = 2f;
    public float hoverAcc = 2f;

    public float lookRateSpeed = 90f;
    public Vector2 lookInput, screenCenter, mouseDistance;

    public float rollInput;
    public float rollSpeed = 90f;
    public float rollAcc = 3.5f;

    private void Awake()
    {
    }

    private void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

        //Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        forwardInput = Input.GetKey(KeyCode.Space) ? 1 : 0;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        //rollInput = Input.GetAxisRaw("Roll");
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
    }
    private void FixedUpdate()
    {
        Movement();
    }
    public void Movement()
    {
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        

        rollInput = Mathf.Lerp(rollInput, -Input.GetAxisRaw("Roll"), rollAcc * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, forwardInput * forwardSpeed, forwardAcc * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, moveInput.x * strafeSpeed, strafeAcc * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, moveInput.y * hoverSpeed, hoverAcc * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
        //transform.Rotate((transform.up * activeStrafeSpeed * Time.deltaTime) + (transform.right * activeHoverSpeed * Time.deltaTime), Space.Self);
    }
}

