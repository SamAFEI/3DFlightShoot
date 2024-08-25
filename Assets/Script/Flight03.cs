using Cinemachine;
using UnityEngine;

public class Flight03 : MonoBehaviour
{
    public Vector2 MoveInput;
    public float yaw { get; private set; }
    public float pitch { get; private set; }
    public float roll { get; private set; }
    public float speed { get; private set; }
    public Rigidbody rb { get; private set; }
    public Transform model { get; private set; }

    private void Awake()
    {
        model = transform.Find("Model");
        rb = GetComponent<Rigidbody>();
        speed = 20f;
    }
    private void Start()
    {
        CamaeraManager.Instance.ChangeCamera(3);
    }

    private void Update()
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void LateUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //rb.velocity = transform.forward * speed;

        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;
        if (Mathf.Abs(posX) > 1000)
        {
            posX *= -1;
        }
        if (Mathf.Abs(posY) > 1000)
        {
            posY *= -1;
        }
        if (Mathf.Abs(posZ) > 1000)
        {
            posZ *= -1;
        }
        transform.position = new Vector3(posX, posY, posZ);

        yaw = MoveInput.x * 10f * Time.deltaTime;
        pitch = Mathf.Lerp(0, 0.05f, Mathf.Abs(MoveInput.y)) * -Mathf.Sin(MoveInput.y);
        roll = Mathf.Lerp(0, 0.1f, Mathf.Abs(MoveInput.x)) * -Mathf.Sin(MoveInput.x);

        //transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);
        transform.Rotate(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

    }

}
