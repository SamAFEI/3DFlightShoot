using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Radar : MonoBehaviour
{
    public static UI_Radar Instance { get; private set; }
    public GameObject testSign;
    public GameObject radarSignRed;
    public GameObject radarSignBlue;
    public List<UI_RadarSign> radarSigns = new List<UI_RadarSign>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    private void LateUpdate()
    {
        UpdateRaderSign(); 
        //TestAngle();
    }

    public static void AddRadarSign(GameObject obj, bool isPlayerFaction)
    {
        GameObject spawnObj = isPlayerFaction ? Instance.radarSignBlue : Instance.radarSignRed;
        UI_RadarSign sign = Instantiate(spawnObj, Instance.transform).GetComponent<UI_RadarSign>();
        sign.keyObj = obj;
        Instance.radarSigns.Add(sign);
    }
    public static void RemoveRadarSign(GameObject obj)
    {
        UI_RadarSign sign = Instance.radarSigns.Where(x => x.keyObj == obj).FirstOrDefault();
        if (sign == null) return;
        Instance.radarSigns.Remove(sign);
        Destroy(sign.gameObject);
    }

    public void TestAngle()
    {
        float width = Screen.width;
        float height = Screen.height;

        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPos = testSign.transform.position;
        screenPos.z = 0;
        float angle = Vector3.SignedAngle(-Vector3.up, screenPos - mousePos, Vector3.forward);
        Debug.Log(angle);
        testSign.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void UpdateRaderSign()
    {
        float width = Screen.width;
        float height = Screen.height;
        foreach (UI_RadarSign sign in radarSigns)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(sign.keyObj.transform.position);
            Vector3 canvasPos = screenPos;
            if (screenPos.z < 0 || canvasPos.x < 0 || canvasPos.x > width || canvasPos.y < 0 || canvasPos.y > height)
            {
                sign.gameObject.SetActive(true);
                float offset = 10;
                canvasPos.x = Mathf.Clamp(screenPos.x, offset, width - offset);
                canvasPos.y = Mathf.Clamp(screenPos.y, offset, height - offset);
                canvasPos.z = 0;
                if (screenPos.z < 0)
                {
                    canvasPos.y = offset;
                }
                sign.transform.position = canvasPos;
                Vector3 vector = transform.position - sign.transform.position;
                vector.z = 0;
                float angle = Vector3.SignedAngle(-Vector3.up, vector, Vector3.forward);
                sign.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                sign.gameObject.SetActive(false);
            }
        }
    }
}
