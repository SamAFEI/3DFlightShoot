﻿using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform Model { get; private set; }
    public Rigidbody RB { get; private set; }
    public GameObject LeftMuzzle { get; private set; }
    public GameObject RightMuzzle { get; private set; }
    public GameObject LaserMuzzle { get; private set; }
    public LineRenderer Laser { get; private set; }
    public GameObject Trail { get; private set; }
    public float yaw { get; private set; }
    public float yawAmount { get; private set; }
    public float pitch { get; private set; }

    public GameObject Bullet;
    public GameObject LaserPerfab;
    public LayerMask RayLayer;
    public TextMeshProUGUI UI_Speed;

    public Vector3 moveInput;
    public Vector2 mouseInput, mouseDistance;

    public float forwardSpeed = 25f;
    public float strafeSpeed = 25f;
    public float hoverSpeed = 25f;

    public float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    public float forwardAcc = 2.5f;
    public float strafeAcc = 2f;
    public float hoverAcc = 2f;
    float forwardInput;

    public float lookRateSpeed = 30f;

    public float rollInput;
    public float rollSpeed = 90f;
    public float rollAcc = 3.5f;

    public float LastAttackTime;
    public float LastCollideTime;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        Model = transform.Find("Model");
        LeftMuzzle = Model.Find("LeftMuzzle").gameObject;
        RightMuzzle = Model.Find("RightMuzzle").gameObject;
        Trail = Model.Find("Trail").gameObject;
        LaserMuzzle = Model.Find("LaserMuzzle").gameObject;
    }
    private void Update()
    {
        LastAttackTime -= Time.deltaTime;
        LastCollideTime -= Time.deltaTime;
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Roll");
        mouseInput = Input.mousePosition;

        if (Input.GetKey(KeyCode.Space)) { forwardInput = 10; }
        if (Input.GetKeyUp(KeyCode.Space)) { forwardInput = 0; }
        forwardInput += moveInput.y * Time.deltaTime * 5f;

        if (Input.GetMouseButton(0) && LastAttackTime < 0f)
        {
            ShootBullet();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootLaser();
        }
        if (Laser != null)
        {
            Laser.SetPosition(0, LaserMuzzle.transform.position);
        }
        Trail.SetActive(activeForwardSpeed > 0);
        UI_Speed.text = "Speed: " + activeForwardSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        activeForwardSpeed = 0;
        forwardInput = 0;
        RB.velocity = Vector3.zero;
        Vector3 vector = transform.position - collision.collider.ClosestPoint(transform.position);
        vector = vector.normalized;
        RB.AddForce(vector * 100f, ForceMode.Impulse);
        LastCollideTime = 0.5f;
    }

    public void Movement()
    {
        if (LastCollideTime > 0)
        {
            forwardInput = 0;
            activeForwardSpeed = 0;
            return;
        }
        RB.velocity = Vector3.zero;
        mouseDistance = CursorPosition();

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, forwardInput * forwardSpeed, forwardAcc * Time.deltaTime);
        activeForwardSpeed = Mathf.Clamp(activeForwardSpeed, 0, 100f);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, moveInput.x * strafeSpeed, strafeAcc * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, moveInput.z * hoverSpeed, hoverAcc * Time.deltaTime);


        //垂直旋轉
        rollInput = Mathf.Lerp(rollInput, moveInput.z, rollAcc * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        //前進
        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        //上下左右 飄移
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        //Model 旋轉 偏移
        yaw += moveInput.x * yawAmount * Time.deltaTime;
        float roll = Mathf.Lerp(0, 10, Mathf.Abs(moveInput.x)) * -Mathf.Sin(moveInput.x);
        Model.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * mouseDistance.x + Vector3.forward * roll);
        float ModelPosX = Mathf.Lerp(Model.localPosition.x, moveInput.x * .3f, .1f);
        Model.localPosition = new Vector3(ModelPosX, Model.localPosition.y, Model.localPosition.z);
    }

    private Vector2 CursorPosition()
    {
        Vector2 cursorPosition = mouseInput;
        cursorPosition.x -= Screen.width * 0.5f;
        cursorPosition.y -= Screen.height * 0.5f;

        float cursorX = cursorPosition.x / (Screen.width * 0.5f);
        float cursorY = cursorPosition.y / (Screen.width * 0.5f);

        float deadZone = 1f;
        if (Mathf.Abs(cursorX) > deadZone) cursorX = 0;
        if (Mathf.Abs(cursorY) > deadZone) cursorY = 0;

        return new Vector2 (cursorX, cursorY);
    }

    private Vector3 CursorDistance()
    {
        Vector3 targetVector = Camera.main.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, 1000f));
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, RayLayer))
        {
            targetVector = hit.point;
            Debug.Log(hit.transform.gameObject.name);
        }
        return targetVector;
    }

    private void ShootBullet()
    {
        LastAttackTime = 0.1f;
        Vector3 targetVector = CursorDistance();
        float bulletSpeed = 1000;
        GameObject leftObj = Instantiate(Bullet, LeftMuzzle.transform.position, Quaternion.identity);
        leftObj.transform.LookAt(targetVector);
        leftObj.GetComponent<Rigidbody>().velocity = leftObj.transform.forward * bulletSpeed;
        GameObject rightObj = Instantiate(Bullet, RightMuzzle.transform.position, Quaternion.identity);
        rightObj.transform.LookAt(targetVector);
        rightObj.GetComponent<Rigidbody>().velocity = rightObj.transform.forward * bulletSpeed;
    }

    private void ShootLaser()
    {
        GameObject obj = Instantiate(LaserPerfab, LaserMuzzle.transform);
        Laser = LaserMuzzle.GetComponentInChildren<LineRenderer>();
        if (Laser != null)
        {
            Laser.SetPosition(0, LaserMuzzle.transform.position);
            Vector3 vector = CursorDistance();
            Laser.SetPosition(1, vector);
        }
        Destroy(obj, 0.1f);
    }
}

