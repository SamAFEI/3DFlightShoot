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

    public UI_ShipStatus uiShipStatus;
    public GameObject explosionPerfab;
    public bool isPlayer;
    public int maxHp = 1000;
    public float forwardThrustPower = 250f;
    public float yawSpeed = 30f;
    public float pitchSpeed = 30f;
    public float rollSpeed = 10f;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        ModelTransform = transform.Find("Model");
        Weapons = GetComponentsInChildren<Weapon>().ToList();
        if (uiShipStatus == null)
        { uiShipStatus = GetComponentInChildren<UI_ShipStatus>(); }
        ControllerInput = GetComponent<IControllerInput>();
        ControllerInput.ForwardEvent += ForwardThrust;
        ControllerInput.HorizontalStrafeEvent += HorizontalStrafeMovement;
        ControllerInput.VerticalStrafeEvent += VerticalStrafeMovement;
        ControllerInput.YawEvent += YawMovement;
        ControllerInput.PitchEvent += PitchMovement;
        ControllerInput.RollEvent += RollMovement;
        ControllerInput.TurnEvent += TurnToTarget;
        ControllerInput.Fire01Event += Fire01Weapon;
        CurrentHp = maxHp;
        isPlayer = GetComponent<PlayerInput>() != null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        RB.velocity = Vector3.zero;
        Vector3 vector = transform.position - collision.collider.ClosestPoint(transform.position);
        vector = vector.normalized;
        RB.AddForce(vector * 50f, ForceMode.Impulse);
    }

    #region Movement
    private void ForwardThrust(float thrust)
    {
        RB.AddForce(transform.forward * thrust * forwardThrustPower * Time.deltaTime);
    }
    private void HorizontalStrafeMovement(float thrust)
    {
        RB.AddForce(transform.right * thrust * forwardThrustPower * Time.deltaTime);
        SetModelOffsetPosition(thrust);
        SetModelOffsetRotation(thrust);
    }
    private void VerticalStrafeMovement(float thrust)
    {
        RB.AddForce(transform.up * thrust * forwardThrustPower * Time.deltaTime);
    }
    private void YawMovement(float yaw)
    {
        RB.AddTorque(transform.up * yaw * yawSpeed * Time.deltaTime);
        SetModelOffsetPosition(-yaw * 3f);
    }
    private void PitchMovement(float pitch)
    {
        RB.AddTorque(transform.right * pitch * pitchSpeed * Time.deltaTime);
    }
    private void RollMovement(float roll)
    {
        RB.AddTorque(transform.forward * roll * rollSpeed * Time.deltaTime);
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

    private float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toOrientation)
    {
        Vector3 vector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float angle = Vector3.SignedAngle(vector, toOrientation, planeNormal);
        return angle;
    }

    public void Hurt(float damage)
    {
        CurrentHp = (int)Mathf.Clamp(CurrentHp - damage, 0, maxHp);
        uiShipStatus.DoLerpHealth();
        if (isPlayer) { CamaeraManager.Shake(3f, .5f); }
        if (CurrentHp <= 0)
        {
            Explosion();
        }
    }

    public void Explosion()
    {
        if (explosionPerfab != null)
        {
            GameObject _explosion = Instantiate(explosionPerfab, transform.position, Quaternion.identity);
            Destroy(_explosion, 5f);
        }
        Destroy(gameObject);
    }
}
