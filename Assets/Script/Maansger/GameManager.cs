using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerObj { get; private set; }
    public List<GameObject> enemyFactions = new List<GameObject>();
    public List<GameObject> playerFactions = new List<GameObject>();

    public int kills;
    public LayerMask enemyFactionLayerMask;
    public LayerMask playerFactionLayerMask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            playerObj = GameObject.FindAnyObjectByType<PlayerInput>().gameObject;
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    #region About Player

    public static Vector3 GetPlayerPosition()
    {
        if (Instance.playerObj == null) { return Vector3.zero; }
        return Instance.playerObj.transform.position;
    }

    /// <summary>
    /// Get 朝Player方向向量
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector3 方向 </returns>
    public static Vector3 GetPlayerDirection(Vector3 _position)
    {
        if (Instance.playerObj == null) { return Vector3.zero; }
        return Instance.playerObj.transform.position - _position;
    }

    /// <summary>
    /// Get 與Player的距離
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public static float GetPlayerDistance(Vector3 _position)
    {
        if (Instance.playerObj == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;

    }
    #endregion

    public static void AddShipList(GameObject obj)
    {
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
}

