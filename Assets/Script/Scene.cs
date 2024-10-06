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
        float minSize = 10.00f;
        float maxSize = 30.00f;
        for (int i = 0; i < 1000; i++)
        {
            Vector3 vector = new Vector3(Random.Range(-Space, Space), Random.Range(-Space, Space), Random.Range(-Space, Space));
            GameObject obj = Instantiate(meteorite, vector, Quaternion.identity);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f), Random.Range(-180.00f, 180.00f)));
            obj.transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
        }
    }
}
