using System.Collections.Generic;
using UnityEngine;

public class Ordinance : MonoBehaviour
{
    public string ordinanceName = "MGBullet";
    public GameObject parentObj;
    public float velocity = 200f;
    public float lifeTime = 2.0f;
    public float armorDamage = 100f;
    public float shieldDamage = 50f;
    public GameObject impact;
    public AudioClip sfxImpact;
    public bool hasHit;

    private void Update()
    {
        if (hasHit) return;
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        List<Vector3> rayDirections = new List<Vector3>()
        {
            transform.forward,
            HelperUtilities.GetDirectionFormAngleInDegrees(1f, transform.forward, transform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(-1f, transform.forward, transform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(1f, transform.forward, transform.up),
            HelperUtilities.GetDirectionFormAngleInDegrees(-1f, transform.forward, transform.up),
        };

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, rayDirections[0], out _hit, 2f + (velocity * 0.02f)) ||
            Physics.Raycast(transform.position, rayDirections[1], out _hit, 2f + (velocity * 0.02f)) ||
            Physics.Raycast(transform.position, rayDirections[2], out _hit, 2f + (velocity * 0.02f)) ||
            Physics.Raycast(transform.position, rayDirections[3], out _hit, 2f + (velocity * 0.02f)) ||
            Physics.Raycast(transform.position, rayDirections[4], out _hit, 2f + (velocity * 0.02f)))
        {
            // Get the target (hit) collider
            Collider _target = _hit.collider;

            // Calculate the incoming vector (hit point minus bullets position since we are not quite yet at the target, only the raycast
            // has detected the hit.
            Vector3 _incomingVec = _hit.point - transform.position;
            // Calculate the reflected vector against the normal (angle) of the surface we hit - this is used to align the bullet impact particle effect.
            Vector3 _reflectVec = Vector3.Reflect(_incomingVec, _hit.normal);

            // Identify the target gameobject that the bullet just hit
            GameObject _targetGameObject = _hit.collider.transform.root.gameObject;

            // Set the bullet position to be the same as the hit point (remember, it's still just the raycast that detected the hit)
            transform.position = _hit.point;

            // Stop the bullet's velocity
            velocity = 0f;

            hasHit = true;

            // Send a "Hit" message to the target object that was hit this doesn't require the target object to listen.
            // If the target object has a public Hit method, e.g. public void Hit(), it will be called when it was hit.
            // You may want to add the ability to send damage or information of a team that you belong to etc. using the
            // second argument which is currently set to null.
            _targetGameObject.SendMessage("Hurt", this, SendMessageOptions.DontRequireReceiver);
            Explode(_hit.point, _reflectVec);
        }
    }
    void OnEnable()
    {
        Destroy(gameObject,lifeTime);
    }

    private void Explode(Vector3 _position, Vector3 _eulerAngles)
    {
        if (impact != null)
        {
            GameObject _impact = Instantiate(impact, _position, Quaternion.identity);
            _impact.transform.forward = _eulerAngles;
            Destroy(_impact, 1f);
            if (sfxImpact != null)
            {
                //AudioSource.PlayClipAtPoint(sfxImpact, _position, 1f);
                AudioManager.PlaySFXOnPoint(sfxImpact, _position, 0.1f);
            }
        }
        Destroy(gameObject);
    }
}
