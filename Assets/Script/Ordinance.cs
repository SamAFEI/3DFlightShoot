using UnityEngine;

public class Ordinance : MonoBehaviour
{
    public string ordinanceName = "MGBullet";
    public float muzzleVelocity = 200f;
    public float armorDamage = 100f;
    public float shieldDamage = 50f;
    public GameObject impact;
    public AudioClip sfxImpact;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.root.SendMessage("Hurt", armorDamage, SendMessageOptions.DontRequireReceiver);
        Explode();
    }

    private void Explode()
    {
        if (impact != null)
        {
            GameObject _impact = Instantiate(impact, transform.position, Quaternion.identity);
            _impact.transform.forward = -transform.forward;
            Destroy(_impact, 1f);
            if (sfxImpact != null)
            {
                AudioSource.PlayClipAtPoint(sfxImpact, transform.position);
            }
        }
        Destroy(gameObject, .05f);
    }
}
