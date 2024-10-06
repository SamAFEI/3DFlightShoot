using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName = "MGGun";
    public float shotsPerSecond = 1000f;
    public GameObject ordinancePrefab;
    public GameObject muzzleFlashPrefab;
    public WeaponBarrel weaponBarrel; 
    public AudioClip sfxFire;
    float coolDownTimer = 0;
    float muzzleVelocity;

    private void Start()
    {
        weaponBarrel = GetComponentInChildren<WeaponBarrel>();
        muzzleVelocity = ordinancePrefab.GetComponent<Ordinance>().muzzleVelocity;
    }

    public void Fire(Vector3 targetVector)
    {
        float coolDownRate = 1 / shotsPerSecond;

        if (coolDownTimer < Time.time)
        {
            coolDownTimer = Time.time + coolDownRate;
            GameObject projectile = Instantiate(ordinancePrefab, weaponBarrel.transform.position, transform.rotation);
            Rigidbody _rb = projectile.GetComponent<Rigidbody>();
            projectile.transform.LookAt(targetVector);
            _rb.velocity = projectile.transform.forward * muzzleVelocity;
            if (sfxFire != null)
            {
                AudioSource.PlayClipAtPoint(sfxFire, transform.position);
            }
        }
    }
}
