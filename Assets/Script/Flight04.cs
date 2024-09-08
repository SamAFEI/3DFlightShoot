using UnityEngine;

public class Flight04 : MonoBehaviour
{
    public Transform model { get; private set; }
    public float forwardInput;
    public float forwardSpeed = 25f;
    public float strafeSpeed = 7.5f;
    public float hoverSpeed = 5f;
    public Vector3 moveInput;

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
        model = transform.Find("Model");
    }
    private void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

        //Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Roll");
    }
    private void FixedUpdate()
    {
        Movement();
    }
    public void Movement()
    {
        mouseDistance.x = (lookInput.x - screenCenter.x) / (screenCenter.x);
        mouseDistance.y = (lookInput.y - screenCenter.y) / (screenCenter.y);

        /*if (Mathf.Abs(mouseDistance.x) < 100f)
        {
            mouseDistance.x = 0f;
        }
        else
        {
            mouseDistance.x = (lookInput.x - screenCenter.x) / (screenCenter.x * 2);
        }
        if (Mathf.Abs(mouseDistance.y) < 100f)
        {
            mouseDistance.y = 0f;
        }
        else
        {
            mouseDistance.y = (lookInput.y - screenCenter.y) / (screenCenter.y * 2);
        }*/

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);
        model.transform.localPosition = new Vector3(-mouseDistance.x * 0.5f, mouseDistance.y * 0.1f, 0f);

        forwardInput = Mathf.Clamp(moveInput.y, 0, 1f);
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, forwardInput * forwardSpeed, forwardAcc * Time.deltaTime);
        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
    }
}

