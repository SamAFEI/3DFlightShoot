using System.Collections;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public GameObject meteorite;
    public GameObject enemyPerfab;
    public GameObject playerFactionPerfab;
    float Space = 2000.00f;

    private void Start()
    {
        Spawn();
        InvokeRepeating("SpawnEnemy", 20f, 30f);
        //InvokeRepeating("SpawnPlayerFaction", 5f, 60f);
    }

    public void Spawn()
    {
        float minSize = 20.00f;
        float maxSize = 40.00f;
        for (int i = 0; i < 2000; i++)
        {
            Vector3 vector = new Vector3(Random.Range(-Space, Space), Random.Range(-Space, Space), Random.Range(-Space, Space));
            GameObject obj = Instantiate(meteorite, vector, Quaternion.identity, transform);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f)));
            obj.transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
        }
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(enemyPerfab, GameManager.GetPlayerPosition() + Random.insideUnitSphere * 1000f, Quaternion.identity);
        }
    }
    public void SpawnPlayerFaction()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(playerFactionPerfab, GameManager.GetPlayerPosition() + Random.insideUnitSphere * 500f, Quaternion.identity);
        }
    }
}
