using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : AIController
{
    public GameObject ordinancePrefab;
    public AudioClip sfxFire;
    float coolDownTimer = 0;
    public float shotsPerSecond = 3f;

    protected override void Start()
    {
        base.Start();
    }

    public override void InitBehaviors()
    {
        //base.InitBehaviors();
        attackAI = new Sequence(new List<BTNode>
        {
            new IsEnemyInView(this, enemyFactionLayerMask),
            new FireWeaponTask(this, Fire)
        });
    }
    public override void AIEvaluate()
    {
        //base.AIEvaluate();
        attackAI.Evaluate();    
    }

    public void Fire(Vector3 targetVector)
    {
        float coolDownRate = 1f / shotsPerSecond;

        if (coolDownTimer < Time.time)
        {
            coolDownTimer = Time.time + coolDownRate;
            transform.LookAt(targetVector);
            GameObject projectile = Instantiate(ordinancePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Ordinance>().parentObj = transform.root.gameObject;
            if (sfxFire != null)
            {
                //AudioSource.PlayClipAtPoint(sfxFire, transform.position);
                AudioManager.PlaySFXOnPoint(sfxFire, transform.position, 0.3f);
            }
        }
    }
}
