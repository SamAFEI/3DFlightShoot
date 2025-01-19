using System.Collections;
using UnityEngine;

public class DepotShip : MonoBehaviour
{
    public GameObject enemyPerfab;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void FixedUpdate()
    {
        //transform.Rotate(new Vector3(0, 0.5f, 0), 10f * Time.deltaTime, Space.Self);
    }


    public IEnumerator Spawn()
    {
        yield return new WaitForSecondsRealtime(10f);
        while (true)
        {
            Vector3 vector = transform.position + (transform.forward * 10);
            GameObject obj = Instantiate(enemyPerfab, vector, Quaternion.LookRotation(transform.forward));
            obj.GetComponent<Rigidbody>().AddRelativeForce(obj.transform.forward * 500f, ForceMode.Impulse);
            yield return new WaitForSecondsRealtime(20f);
        }
    }
}
