using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerObj { get; private set; }
    public Player player { get; private set; }

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

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        if (playerObj != null) { player = playerObj.GetComponent<Player>(); }
    }

    #region About Player

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
    #endregion
}
