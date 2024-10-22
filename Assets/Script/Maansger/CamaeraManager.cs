using Cinemachine;
using UnityEngine;

public class CamaeraManager : MonoBehaviour
{
    public static CamaeraManager Instance { get; private set; }
    public CinemachineBrain cinemachineBrain { get; private set; }
    public CinemachineVirtualCamera firstCamera { get; private set; }
    public CinemachineVirtualCamera secondCamera { get; private set; }
    public CinemachineVirtualCamera thirdCamera { get; private set; }
    public CinemachineVirtualCamera activeCamera { get; private set; }
    public CinemachineBasicMultiChannelPerlin cbmPerlin { get; private set; }

    private float shakeTimer = 0;
    private float shakeTimerTotal;
    private float startingIntensity;
    private bool isSharking;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        firstCamera = transform.Find("FirstCamera").GetComponent<CinemachineVirtualCamera>();
        secondCamera = transform.Find("SecondCamera").GetComponent<CinemachineVirtualCamera>();
        thirdCamera = transform.Find("ThirdCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        //SetActiveCamera();
    }

    private void Update()
    {
        //可能第一幀為null
        if (activeCamera == null) { SetActiveCamera(); }

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
        DoShake();
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
        SetActiveCamera();
    }
    public void SetActiveCamera()
    {
        activeCamera = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (activeCamera == null) return;
        cbmPerlin = activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmPerlin.m_AmplitudeGain = 0f;
        cbmPerlin.m_FrequencyGain = 0f;
    }

    #region ShakeCamera
    public static void Shake(float intensity, float time)
    {
        if (Instance.isSharking) { return; }
        if (Instance.shakeTimer <= 0)
        {
            Instance.startingIntensity = intensity;
            Instance.shakeTimer = time;
            Instance.shakeTimerTotal = time;
            Instance.isSharking = true;
        }
    }
    public void DoShake()
    {
        if (shakeTimer > 0)
        {
            SetActiveCamera();
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                cbmPerlin.m_AmplitudeGain = 0f;
                cbmPerlin.m_FrequencyGain = 0f;
                isSharking = false;
            }
            else
            {
                cbmPerlin.m_FrequencyGain = 0.1f;
                cbmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / -shakeTimerTotal);
            }
        }
        //Camera.main.transform.rotation = Quaternion.identity;
    }
    #endregion
}
