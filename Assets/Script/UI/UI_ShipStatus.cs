using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShipStatus : MonoBehaviour
{
    private RectTransform myRectTransform => GetComponent<RectTransform>();
    public Slider hpSlider { get; private set; }
    public bool isPlayerUI;
    public ShipController shipController;
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
    }

    private void Start()
    {
        if (!isPlayerUI)
        { shipController = GetComponentInParent<ShipController>(); }
        hpSlider.maxValue = shipController.maxHp;
        hpSlider.value = hpSlider.maxValue;
        hpSlider.gameObject.SetActive(isPlayerUI);
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

    /// <summary>
    /// ­±¦VCamera
    /// </summary>
    protected void DoBillboard()
    {
        myRectTransform.rotation = Camera.main.transform.rotation;
        myRectTransform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
