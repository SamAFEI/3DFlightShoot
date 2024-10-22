using UnityEngine;

public class PlayerInput : MonoBehaviour, IControllerInput
{
    public ShipController ShipController { get; private set; }
    public event InputEventFloat ForwardEvent;
    public event InputEventFloat HorizontalStrafeEvent;
    public event InputEventFloat VerticalStrafeEvent;
    public event InputEventFloat YawEvent;
    public event InputEventFloat PitchEvent;
    public event InputEventFloat RollEvent;
    public event InputEventVector3 Fire01Event;
    public event InputEventVector3 Fire02Event;
    public event InputEventVector3 TurnEvent;

    public LayerMask enemyLayerMask;
    public float deadZoneRadius = 1f;
    public float invertModifier = -1f;
    public UI_Health uiHealth;
    public GameObject targetObj;
    public float lastLocateTime;

    private void Awake()
    {
        ShipController = GetComponent<ShipController>();
    }
    void Update()
    {
        lastLocateTime -= Time.deltaTime;
        if (lastLocateTime < 0f) 
        { 
            targetObj = null; 
        }
        if (ShipController.IsDie) { return; }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShipController.CurrentHp = 0;
            ShipController.Explosion();
        }
        GetKeyboardInput();
        GetMouseInput();
    }

    private void GetKeyboardInput()
    {
        if (ForwardEvent != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ForwardEvent(2);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                ForwardEvent(Input.GetAxis("Vertical"));
            }
        }
        if (HorizontalStrafeEvent != null)
        {
            HorizontalStrafeEvent(Input.GetAxis("Horizontal"));
        }
        if (VerticalStrafeEvent != null)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                //VerticalStrafeEvent(Input.GetAxis("Vertical"));
            }
        }
        if (RollEvent != null)
        {
            if (Input.GetAxis("Roll") != 0)
            {
                RollEvent(Input.GetAxis("Roll"));
            }
        }
    }
    private void GetMouseInput()
    {
        Vector3 mousePos = Input.mousePosition;
        float yaw = (mousePos.x - (Screen.width * .5f)) / (Screen.width * .5f);
        if (Mathf.Abs(yaw) < deadZoneRadius) yaw = 0.0f;
        if (YawEvent != null)
        {
            YawEvent(yaw);
        }
        float pitch = (mousePos.y - (Screen.height * .5f)) / (Screen.height * .5f) * invertModifier;
        if (Mathf.Abs(pitch) < deadZoneRadius) pitch = 0.0f;
        if (PitchEvent != null)
        {
            if (pitch != 0)
            {
                PitchEvent(pitch);
            }
        }

        Vector3 fireVector = CursorDistance(mousePos);
        if (Fire01Event != null)
        {
            if (Input.GetMouseButton(0))
            {
                Fire01Event(fireVector);
            }
        }
    }

    private Vector3 CursorDistance(Vector2 cursor)
    {
        Vector3 targetVector = Camera.main.ScreenToWorldPoint(new Vector3(cursor.x, cursor.y, 1000f));
        Ray ray = Camera.main.ScreenPointToRay(cursor);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
        {
            lastLocateTime = 0.3f;
            targetObj = hit.collider.transform.root.gameObject;
        }
        uiHealth.gameObject.SetActive(false);
        if (targetObj != null) 
        {
            targetVector = targetObj.transform.position;
            float distance = (targetVector - transform.position).magnitude;
            float offset = targetObj.GetComponent<Rigidbody>().velocity.magnitude * distance / 1000f;
            Vector3 velocity = targetObj.GetComponent<Rigidbody>().velocity.normalized * offset;
            targetVector = targetVector + velocity;
            uiHealth.gameObject.SetActive(true);
            uiHealth.SetShip(targetObj.GetComponent<ShipController>());
        }
        return targetVector;
    }
}
