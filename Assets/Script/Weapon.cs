using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName = "MGGun";
    public float shotsPerSecond = 4f;
    public GameObject ordinancePrefab;
    public GameObject muzzleFlashPrefab;
    public WeaponBarrel weaponBarrel; 
    public AudioClip sfxFire;
    float coolDownTimer = 0;

    private void Start()
    {
        weaponBarrel = GetComponentInChildren<WeaponBarrel>();
    }

    public void Fire(Vector3 targetVector)
    {
        float coolDownRate = 1f / shotsPerSecond;

        if (coolDownTimer < Time.time)
        {
            coolDownTimer = Time.time + coolDownRate;
            transform.LookAt(targetVector);
            GameObject projectile = Instantiate(ordinancePrefab, weaponBarrel.transform.position, transform.rotation);
            projectile.GetComponent<Ordinance>().parentObj = transform.root.gameObject;
            if (sfxFire != null)
            {
                //AudioSource.PlayClipAtPoint(sfxFire, transform.position);
                AudioManager.PlaySFXOnPoint(sfxFire, transform.position, 0.3f);
            }
        }
    }
}
