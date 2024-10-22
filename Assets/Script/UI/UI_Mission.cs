using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Mission : MonoBehaviour
{
    public static UI_Mission Instance { get; private set; }

    public TextMeshProUGUI tmp_MissionComplete { get; private set; }
    public TextMeshProUGUI tmp_MissionFails { get; private set; }
    public Button bt_Restart { get; private set; }
    public TextMeshProUGUI tmp_Restart { get; private set; }
    public bool isTigger;

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

        tmp_MissionComplete = transform.Find("TMP_MissionComplete").GetComponent<TextMeshProUGUI>();
        tmp_MissionFails = transform.Find("TMP_MissionFails").GetComponent<TextMeshProUGUI>();
        bt_Restart = transform.Find("BT_Restart").GetComponent<Button>();
        bt_Restart.onClick.AddListener(() => GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name));
        tmp_Restart = bt_Restart.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetMissionResult(bool complete)
    {
        tmp_MissionComplete.enabled = complete;
        tmp_MissionFails.enabled = !complete;
        if (!isTigger)
        {
            if (complete)
            {
                StartCoroutine(FadeInTMP(tmp_MissionComplete));
                tmp_Restart.color = tmp_MissionComplete.color;
            }
            else
            {
                StartCoroutine(FadeInTMP(tmp_MissionFails));
                tmp_Restart.color = tmp_MissionFails.color; 
            }
            StartCoroutine(FadeBT_Restart());
            isTigger = true;
        }
    }
    private IEnumerator FadeBT_Restart()
    {
        while (true)
        {
            for (float alpha = 0.1f; alpha < 1f; alpha += Time.deltaTime)
            {
                tmp_Restart.color = new Color(tmp_Restart.color.r, tmp_Restart.color.g, tmp_Restart.color.b, alpha);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            for (float alpha = 1f; alpha > 0.1f; alpha -= Time.deltaTime)
            {
                tmp_Restart.color = new Color(tmp_Restart.color.r, tmp_Restart.color.g, tmp_Restart.color.b, alpha);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator FadeInTMP(TextMeshProUGUI tmp)
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        for (float alpha = 0.1f; alpha < 1f; alpha += Time.deltaTime)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            yield return null;
        }
    }
}
