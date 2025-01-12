using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static string LoadSceneName;

    public List<GameObject> enemyFactions = new List<GameObject>();
    public List<GameObject> playerFactions = new List<GameObject>();

    public bool isDemo;
    public GameObject playerObj;
    public Vector3 playerPos;
    public int kills;
    public LayerMask enemyFactionLayerMask;
    public LayerMask playerFactionLayerMask;
    public UI_Mission UI_Mission;
    public Image UI_Locked;
    public float playerBeLoctedTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            if (playerObj == null)
            { playerObj = GameObject.FindAnyObjectByType<PlayerInput>().gameObject; }
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(DoFlashLoced());
    }

    private void Update()
    {
        playerBeLoctedTime -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (Instance.playerObj != null)
        {
            Instance.playerPos = Instance.playerObj.transform.position;
        }
        if (enemyFactions.Count == 0)
        {
            SetMission(true);
        }
    }

    #region About Player

    public static Vector3 GetPlayerPosition()
    {
        return Instance.playerPos;
    }

    /// <summary>
    /// Get 朝Player方向向量
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector3 方向 </returns>
    public static Vector3 GetPlayerDirection(Vector3 _position)
    {
        return Instance.playerPos - _position;
    }

    /// <summary>
    /// Get 與Player的距離
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public static float GetPlayerDistance(Vector3 _position)
    {
        return GetPlayerDirection(_position).magnitude;

    }
    #endregion

    public static void AddShipList(GameObject obj)
    {
        if (Instance.isDemo) { return; }
        if ((Instance.enemyFactionLayerMask & (1 << obj.layer)) != 0)
        {
            Instance.enemyFactions.Add(obj);
            UI_Radar.AddRadarSign(obj, false);
        }
        else if ((Instance.playerFactionLayerMask & (1 << obj.layer)) != 0)
        {
            Instance.playerFactions.Add(obj);
            UI_Radar.AddRadarSign(obj, true);
        }
    }
    public static void RemoveShipList(GameObject obj)
    {
        if (Instance.isDemo) { return; }
        if ((Instance.enemyFactionLayerMask & (1 << obj.layer)) != 0)
        {
            Instance.enemyFactions.Remove(obj);
        }
        else if ((Instance.playerFactionLayerMask & (1 << obj.layer)) != 0)
        {
            Instance.playerFactions.Remove(obj);
        }
        UI_Radar.RemoveRadarSign(obj);
    }

    public static void UpdateKills()
    {
        Instance.kills++;
    }

    public static void BeLocated()
    {
        Instance.playerBeLoctedTime = 0.5f;
    }

    public void LoadScene(string _sceneName)
    {
        LoadSceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void SetMission(bool success)
    {
        if (UI_Mission != null)
        {
            UI_Mission.gameObject.SetActive(true);
            UI_Mission.SetMissionResult(success);
        }
    }

    public IEnumerator DoFlashLoced()
    {
        while (true)
        {
            while (playerBeLoctedTime > 0)
            {
                UI_Locked.color = new Color(UI_Locked.color.r, UI_Locked.color.g, UI_Locked.color.b, 1);
                yield return null;
                UI_Locked.color = new Color(UI_Locked.color.r, UI_Locked.color.g, UI_Locked.color.b, 0);
                yield return null;
            }
            UI_Locked.color = new Color(UI_Locked.color.r, UI_Locked.color.g, UI_Locked.color.b, 0);
            yield return new WaitForSeconds(1f);
        }
    }
}

