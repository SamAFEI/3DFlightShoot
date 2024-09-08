using UnityEngine;

public class Scene : MonoBehaviour
{
    public GameObject meteorite;
    float Space = 1000.00f;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = 0; i < 5000; i++)
        {
            Vector3 vector = new Vector3(Random.Range(-Space, Space), Random.Range(-Space, Space), Random.Range(-Space, Space));
            GameObject obj = Instantiate(meteorite, vector, Quaternion.identity);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f)));
            obj.transform.localScale = new Vector3(Random.Range(5.00f, 10.00f), Random.Range(5.00f, 10.00f), Random.Range(5.00f, 10.00f));
        }
    }
}
