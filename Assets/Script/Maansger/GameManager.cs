using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerObj { get; private set; }

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
    /// Get ��Player��V�V�q
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector3 ��V </returns>
    public static Vector3 GetPlayerDirection(Vector3 _position)
    {
        if (Instance.playerObj == null) { return Vector3.zero; }
        return Instance.playerObj.transform.position - _position;
    }

    /// <summary>
    /// Get �PPlayer���Z��
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public static float GetPlayerDistance(Vector3 _position)
    {
        if (Instance.playerObj == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;

    }
    #endregion
}
