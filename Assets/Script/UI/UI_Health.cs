using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    public Slider hpSlider { get; private set; }
    public ShipController shipController;
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }
    public void DoLerpHealth(ShipController ship)
    {
        if (ship.gameObject.name != shipController.gameObject.name) { return; }
        hpSmooth = 0;
        StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        hpSlider.maxValue = shipController.maxHp;
        hpSlider.value = hpSlider.maxValue;
        float smooth = 50;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            //hpSlider.gameObject.SetActive(true);
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, shipController.CurrentHp, hpSmooth);
            yield return null;
        }
        //yield return new WaitForSeconds(2f);
        //hpSlider.gameObject.SetActive(false);
    }

    public void SetShip(ShipController ship)
    {
        shipController = ship; 
        hpSlider.maxValue = shipController.maxHp;
        hpSlider.value = shipController.CurrentHp;
    }
}
