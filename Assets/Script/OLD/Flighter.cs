using UnityEditor.Rendering;
using UnityEngine;

public class Flighter : MonoBehaviour
{
    public Vector2 MoveInput;
    public Transform model { get; private set; }
    public float rollZ { get; private set; }
    public float rollX { get; private set; }
    public float speed { get; private set; }

    private void Awake()
    {
        model = transform.Find("FighterModel").transform;
        rollZ = -20f;
        rollX = -10f;
        speed = 3f;
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
        if (MoveInput == Vector2.zero)
        {
            model.localRotation = Quaternion.identity;
            return;
        }
        model.localRotation = Quaternion.Euler(new Vector3(MoveInput.y * rollX, 0, MoveInput.x * rollZ));

        if ((transform.localPosition.x > 2 && MoveInput.x > 0)
            || (transform.localPosition.x < -2 && MoveInput.x < 0))
        {
            MoveInput.x = 0;
        }
        if ((transform.localPosition.y > 1 && MoveInput.y > 0) 
            || (transform.localPosition.y < -1.3f && MoveInput.y < 0))
        {
            MoveInput.y = 0;
        }
        Vector2 vector = MoveInput.normalized * speed * Time.deltaTime;
        transform.Translate(vector);
    }
}
