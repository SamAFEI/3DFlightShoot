using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Rigidbody RB { get; private set; }
    public IControllerInput ControllerInput { get; private set; }
    public Transform ModelTransform { get; private set; }
    public List<Weapon> Weapons { get; private set; } = new List<Weapon>();
    public int CurrentHp { get; set; }
    public float CurrentEp { get; set; }
    public float CurrentBullets { get; set; }
    public bool IsDie { get { return CurrentHp <= 0; } }

    public UI_Canvas uiCanvas { get; private set; }
    public UI_Health uiHealth { get; private set; }
    public UI_Health uiEnergy { get; private set; }
    public UI_ShipStatus uiShipStatus;
    public GameObject explosionPerfab;
    public AudioClip explosionClip;
    public bool isPlayer;
    public int maxHp = 500;
    public int maxEp = 500;
    public int maxBullets = 500;
    public float forwardThrustPower = 2000f;
    public float yawSpeed = 500f;
    public float pitchSpeed = 300f;
    public float rollSpeed = 500f;
    public bool isRolling;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        ModelTransform = transform.Find("Model");
        Weapons = GetComponentsInChildren<Weapon>().ToList();
        if (uiShipStatus == null)
        { uiShipStatus = GetComponentInChildren<UI_ShipStatus>(); }
        ControllerInput = GetComponent<IControllerInput>();
        if (ControllerInput != null)
        {
            ControllerInput.ForwardEvent += ForwardThrust;
            ControllerInput.HorizontalStrafeEvent += HorizontalStrafeMovement;
            ControllerInput.VerticalStrafeEvent += VerticalStrafeMovement;
            ControllerInput.YawEvent += YawMovement;
            ControllerInput.PitchEvent += PitchMovement;
            ControllerInput.RollEvent += RollMovement;
            ControllerInput.TurnEvent += TurnToTarget;
            ControllerInput.Fire01Event += Fire01Weapon;
        }
        CurrentHp = maxHp;
        CurrentEp = maxEp;
        CurrentBullets = maxBullets;
        isPlayer = GetComponent<PlayerInput>() != null;
    }

    private void Start()
    {
        GameManager.AddShipList(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        RB.velocity = Vector3.zero;
        Vector3 vector = transform.position - collision.collider.ClosestPoint(transform.position);
        vector = vector.normalized;
        RB.AddForce(vector * RB.mass * 100f, ForceMode.Impulse);
    }

    #region Movement
    private void ForwardThrust(float thrust)
    {
        Vector3 power = transform.forward * thrust * forwardThrustPower * Time.deltaTime;
        RB.AddForce(power);
    }
    private void HorizontalStrafeMovement(float thrust)
    {
        RB.AddForce(transform.right * thrust * forwardThrustPower * 2f * Time.deltaTime);
        SetModelOffsetPosition(thrust);
        SetModelOffsetRotation(thrust);
        SetUIShipStatusOffsetPosition(thrust);
        //SetUIShipStatusOffsetRotation(thrust);
    }
    private void VerticalStrafeMovement(float thrust)
    {
        RB.AddForce(transform.up * thrust * forwardThrustPower * Time.deltaTime);
    }
    private void YawMovement(float yaw)
    {
        RB.AddTorque(transform.up * yaw * yawSpeed * Time.deltaTime);
        SetModelOffsetPosition(-yaw * 6f);
        SetUIShipStatusOffsetPosition(yaw);
        if (!isRolling)
        {
            SetUIShipStatusOffsetRotation(yaw);
        }
    }
    private void PitchMovement(float pitch)
    {
        RB.AddTorque(transform.right * pitch * pitchSpeed * Time.deltaTime);
    }
    private void RollMovement(float roll)
    {
        RB.AddTorque(transform.forward * roll * rollSpeed * Time.deltaTime);
        SetUIShipStatusOffsetRotation(-roll);
    }
    #endregion

    private void TurnToTarget(Vector3 desiredHeading)
    {
        Quaternion rotationGoal = Quaternion.LookRotation(desiredHeading);

        float step = yawSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationGoal, step);
    }
    private void Fire01Weapon(Vector3 fireTarget)
    {
        
        if (Weapons.Count > 0)
        {
            foreach (var weapon in Weapons)
            {
                weapon.Fire(fireTarget);
            }
        }
    }
    private void SetModelOffsetPosition(float power)
    {
        float ModelPosX = Mathf.Lerp(ModelTransform.localPosition.x, power * .5f, Time.deltaTime);
        ModelTransform.localPosition = new Vector3(ModelPosX, ModelTransform.localPosition.y, ModelTransform.localPosition.z);
    }
    private void SetModelOffsetRotation(float power)
    {
        float roll = Mathf.Lerp(0, 20, Mathf.Abs(power)) * -Mathf.Sin(power);
        ModelTransform.localRotation = Quaternion.Euler(Vector3.up + Vector3.right + Vector3.forward * roll);
    }
    public void SetUIShipStatusOffsetPosition(float power)
    {
        Vector3 uiStatePos = uiShipStatus.transform.localPosition;
        float uiStatePosX = Mathf.Lerp(uiStatePos.x, power * 150f, Time.deltaTime);
        uiShipStatus.transform.localPosition = new Vector3(uiStatePosX, uiStatePos.y, uiStatePos.z);
    }
    public void SetUIShipStatusOffsetRotation(float power)
    {
        float roll = Mathf.Lerp(0, 20, Mathf.Abs(power)) * -Mathf.Sin(power);
        uiShipStatus.transform.localRotation = Quaternion.Euler(Vector3.up + Vector3.right + Vector3.forward * roll);
    }
    public void SetUIShipStatusOffsetScale(float power)
    {
        Vector3 uiStateScale = uiShipStatus.transform.localScale;
        float scale = Mathf.Lerp(uiStateScale.x, power, Time.deltaTime);
        scale = Mathf.Clamp(scale, 0.6f, 1.1f);
        uiShipStatus.transform.localScale = new Vector3(scale, scale, scale);
    }
    private float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toOrientation)
    {
        Vector3 vector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float angle = Vector3.SignedAngle(vector, toOrientation, planeNormal);
        return angle;
    }
    public void Hurt(Ordinance bullet)
    {
        CurrentHp = (int)Mathf.Clamp(CurrentHp - bullet.armorDamage, 0, maxHp);
        if (isPlayer) 
        { 
            CamaeraManager.Shake(3f, .5f); 
            uiShipStatus.DoLerpHealth();
        }
        SendMessage("DoLerpHealth", this, SendMessageOptions.DontRequireReceiver);
        if (IsDie)
        {
            if (bullet.parentObj == GameManager.Instance.playerObj)
            {
                GameManager.UpdateKills();
            }
            Explosion();
        }
    }
    public void ConsumeEnergy(float power)
    {
        CurrentEp = Mathf.Clamp(CurrentEp - power, 0, maxEp);
        uiShipStatus.DoLerpEnergy();
        //SendMessage("DoLerpEnergy", this, SendMessageOptions.DontRequireReceiver);
    }
    public void ConsumeBullets(float amount)
    {
        CurrentBullets = Mathf.Clamp(CurrentBullets - amount, 0, maxEp);
        uiShipStatus.DoLerpBullets();
    }
    public void Explosion()
    {
        if (explosionPerfab != null)
        {
            GameObject _explosion = Instantiate(explosionPerfab, transform.position, Quaternion.identity);
            AudioManager.PlaySFXOnPoint(explosionClip, transform.position, 0.8f);
            Destroy(_explosion, 5f);
        }
        GameManager.RemoveShipList(this.gameObject);
        gameObject.SetActive(false);
        if (isPlayer)
        {
            GameManager.Instance.SetMission(false);
        }
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
    }
}
