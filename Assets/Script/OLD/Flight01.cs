using UnityEngine;

public class Flight01 : MonoBehaviour
{
    public Vector2 MoveInput;
    public float yaw { get; private set; }
    public float yawAmount { get; private set; }
    public float pitch { get; private set; }
    public float speed { get; private set; }
    public Rigidbody rb { get; private set; }
    public Transform model { get; private set; }

    private void Awake()
    {
        model = transform.Find("Model");
        rb = GetComponent<Rigidbody>();
        speed = 5f;
        yawAmount = 30f;
    }

    private void Update()
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");

        Movement();
    }

    private void LateUpdate()
    {
    }

    public void Movement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;   
        //rb.velocity = transform.forward * speed;

        yaw += MoveInput.x * yawAmount * Time.deltaTime;
        pitch += Mathf.Lerp(0, 0.1f, Mathf.Abs(MoveInput.y)) * -Mathf.Sin(MoveInput.y) ;
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(MoveInput.x)) * -Mathf.Sin(MoveInput.x);

        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);
    }
}
