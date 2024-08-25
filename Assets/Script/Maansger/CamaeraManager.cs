using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamaeraManager : MonoBehaviour
{
    public static CamaeraManager Instance { get; private set; }
    public CinemachineBrain cinemachineBrain { get; private set; }
    public CinemachineVirtualCamera firstCamera { get; private set; }
    public CinemachineVirtualCamera secondCamera { get; private set; }
    public CinemachineVirtualCamera thirdCamera { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        firstCamera = transform.Find("FirstCamera").GetComponent<CinemachineVirtualCamera>();
        secondCamera = transform.Find("SecondCamera").GetComponent<CinemachineVirtualCamera>();
        thirdCamera = transform.Find("ThirdCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCamera(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCamera(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeCamera(3);
        }
    }

    public void ChangeCamera(int _index)
    {
        cinemachineBrain.ActiveVirtualCamera.Priority = 10;
        if (_index == 1)
        {
            firstCamera.Priority = 11;
        }
        else if (_index == 2)
        {
            secondCamera.Priority = 11;
        }
        else if (_index == 3)
        {
            thirdCamera.Priority = 11;
        }
    }
}
