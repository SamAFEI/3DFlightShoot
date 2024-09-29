using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ordinance : MonoBehaviour
{
    public string ordinanceName = "MGBullet";
    public float muzzleVelocity = 300f;
    public float armorDamage = 100f;
    public float shieldDamage = 50f;
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();    
    }

    private void Explode()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        //Destroy(gameObject, .05f);
    }
}
