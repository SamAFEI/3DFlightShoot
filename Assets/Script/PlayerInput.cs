using TMPro;
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
    public TextMeshProUGUI tmpDistance;
    public float lastLocateTime;
    public float epRechargeTime;
    public float lastFireTime;
    public bool isWarpDrive;
    public ParticleSystem FX_Flames;
    public ParticleSystem FX_FlamesBig;

    private void Awake()
    {
        ShipController = GetComponent<ShipController>();
    }
    void Update()
    {
        lastLocateTime -= Time.deltaTime;
        epRechargeTime -= Time.deltaTime;
        lastFireTime -= Time.deltaTime;
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
    private void LateUpdate()
    {
        if (epRechargeTime <= 0)
        {
            ShipController.ConsumeEnergy(-3f);
        }
        if (lastFireTime <= 0)
        {
            ShipController.ConsumeBullets(-3f);
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
                    AudioManager.NoEPSFX();
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
            if (power == 0.6f)
            {
                FX_Flames.Stop();
                FX_FlamesBig.Play();
            }
            else if (power == 0.8f)
            {
                FX_Flames.Play();
                FX_FlamesBig.Stop();
            }
            else
            {
                FX_Flames.Stop();
                FX_FlamesBig.Stop();
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
                ShipController.ConsumeBullets(0.5f);
                lastFireTime = 0.1f;
                if (ShipController.CurrentBullets <= 0)
                {
                    AudioManager.NoFireSFX();
                    lastFireTime = 0.5f;
                }
                else
                {
                    Fire01Event(fireVector);
                }
            }
            
        }
    }

    private Vector3 CursorDistance(Vector2 cursor)
    {
        Vector3 cursorVector = Camera.main.ScreenToWorldPoint(new Vector3(cursor.x, cursor.y, 1000f));
        Vector3 targetVector = cursorVector;
        Ray ray = Camera.main.ScreenPointToRay(cursor);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
        {
            lastLocateTime = 0.3f;
            targetObj = hit.collider.transform.root.gameObject;
        }
        uiHealth.gameObject.SetActive(false);
        tmpDistance.gameObject.SetActive(false);
        if (targetObj != null) 
        {
            float offset;
            Vector3 velocity;
            targetVector = targetObj.transform.position;
            float distance = (targetVector - transform.position).magnitude; 
            distance = (int)distance;
            float scale = 1;
            tmpDistance.text = distance.ToString();
            tmpDistance.gameObject.SetActive(true);
            if (distance >= 400)
            {
                offset = distance / 100f;
                //if (offset == 0) { offset = distance / 300f; offset *= offset; }
                offset *= Random.Range(-2.0f, 2.0f);
                velocity = cursorVector.normalized * offset;
                targetVector = cursorVector + velocity;
                return targetVector; 
            }
            else
            {
                offset = targetObj.GetComponent<Rigidbody>().velocity.magnitude * distance / 1000f;
                velocity = targetObj.GetComponent<Rigidbody>().velocity.normalized * offset;
                targetVector = targetVector + velocity;
            }
            scale = Mathf.Clamp(1 - 0.0025f * (distance - 200), 0.5f, 1f); //ChatGPT 算的
            uiHealth.gameObject.transform.localScale = Vector3.one * scale;
            uiHealth.gameObject.SetActive(true);
            uiHealth.SetShip(targetObj.GetComponent<ShipController>());
            
        }
        return targetVector;
    }
}
