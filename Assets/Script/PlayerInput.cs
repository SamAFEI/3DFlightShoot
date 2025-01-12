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

    public ParticleSystem WarpDriveFVX;
    public LayerMask enemyLayerMask;
    public float deadZoneRadius = 1f;
    public float invertModifier = -1f;
    public UI_Health uiHealth;
    public GameObject targetObj;
    public UI_Slot slot;
    public float lastLocateTime;
    public float epRechargeTime;
    public bool isWarpDrive;

    private void Awake()
    {
        ShipController = GetComponent<ShipController>();
    }
    void Update()
    {
        lastLocateTime -= Time.deltaTime;
        epRechargeTime -= Time.deltaTime;
        if (lastLocateTime < 0f) 
        { 
            targetObj = null;
        }
        if (epRechargeTime > 40) //從 5f 扣到負數會變成4X??? 不懂為啥強制給 0
        {
            epRechargeTime = 0f;
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
    private void LateUpdate()
    {
        if (epRechargeTime <= 0)
        {
            ShipController.ConsumeEnergy(-1f);
        }
    }

    private void GetKeyboardInput()
    {
        if (ForwardEvent != null)
        {
            float power = 1; 
            if (Input.GetAxis("Vertical") != 0)
            {
                ForwardEvent(Input.GetAxis("Vertical"));
                power = 1 - Input.GetAxis("Vertical") * 0.2f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                isWarpDrive = true;
                ShipController.ConsumeEnergy(0.5f);
                epRechargeTime = 0.1f;
                if (ShipController.CurrentEp <= 0)
                {
                    AudioManager.NoFireSFX();
                    isWarpDrive = false;
                    epRechargeTime = 0.5f;
                }
                else
                {
                    ForwardEvent(2);
                    power = 0.6f;
                }
            }
            else 
            {
                isWarpDrive = false;
            }
            ShipController.SetUIShipStatusOffsetScale(power);
        }
        if (WarpDriveFVX != null)
        {
            if (isWarpDrive && !WarpDriveFVX.isPlaying)
            {
                WarpDriveFVX.Play();
            }
            else if (!isWarpDrive && WarpDriveFVX.isPlaying)
            {
                WarpDriveFVX.Stop();
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
            ShipController.isRolling = Input.GetAxis("Roll") != 0;
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
            //if (pitch != 0)
            {
                PitchEvent(pitch);
            }
        }

        Vector3 fireVector = CursorDistance(mousePos);
        if (Fire01Event != null)
        {
            slot.DoFlash(Input.GetMouseButton(0));
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
