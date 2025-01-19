using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShipStatus : MonoBehaviour
{
    private RectTransform myRectTransform => GetComponent<RectTransform>();
    public Slider hpSlider { get; private set; }
    public Slider epSlider;
    public Slider bulletsSlider;
    public bool isPlayerUI;
    public ShipController shipController;
    private float hpSmooth, epSmooth, bulletsSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>(); ;
    }

    private void Start()
    {
        if (!isPlayerUI)
        { shipController = GetComponentInParent<ShipController>(); }
        hpSlider.maxValue = shipController.maxHp;
        hpSlider.value = hpSlider.maxValue;
        hpSlider.gameObject.SetActive(isPlayerUI);
        if (epSlider != null )
        {
            epSlider.maxValue = shipController.maxEp;
            epSlider.value = epSlider.maxValue;
            epSlider.gameObject.SetActive(isPlayerUI);
        }
        if (bulletsSlider != null)
        {
            bulletsSlider.maxValue = shipController.maxBullets;
            bulletsSlider.value = bulletsSlider.maxValue;
            bulletsSlider.gameObject.SetActive(isPlayerUI);
        }
    }

    private void Update()
    {
        if (!isPlayerUI) DoBillboard();
    }

    public void DoLerpHealth()
    {
        hpSmooth = 0;
        StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        float smooth = 50;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            hpSlider.gameObject.SetActive(true);
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, shipController.CurrentHp, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        hpSlider.gameObject.SetActive(isPlayerUI);
    }

    public void DoLerpEnergy()
    {
        if (epSlider == null) return;
        epSmooth = 0;
        StartCoroutine(LerpEnergy());
    }

    private IEnumerator LerpEnergy()
    {
        float smooth = 50;
        float startEP = epSlider.value;
        while (epSmooth < 1)
        {
            epSlider.gameObject.SetActive(true);
            epSmooth += Time.deltaTime * smooth;
            epSlider.value = Mathf.Lerp(startEP, shipController.CurrentEp, epSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        epSlider.gameObject.SetActive(isPlayerUI);
    }

    public void DoLerpBullets()
    {
        if (bulletsSlider == null) return;
        bulletsSmooth = 0;
        StartCoroutine(LerpBullets());
    }

    private IEnumerator LerpBullets()
    {
        float smooth = 50;
        float startBullets = bulletsSlider.value;
        while (bulletsSmooth < 1)
        {
            bulletsSlider.gameObject.SetActive(true);
            bulletsSmooth += Time.deltaTime * smooth;
            bulletsSlider.value = Mathf.Lerp(startBullets, shipController.CurrentBullets, bulletsSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        epSlider.gameObject.SetActive(isPlayerUI);
    }


    /// <summary>
    /// 面向Camera
    /// </summary>
    protected void DoBillboard()
    {
        myRectTransform.rotation = Camera.main.transform.rotation;
        myRectTransform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
