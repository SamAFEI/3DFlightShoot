using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    public TextMeshProUGUI ui_PlayerFaction { get; private set; }
    public TextMeshProUGUI ui_EnemyFaction { get; private set; }
    public TextMeshProUGUI ui_Kills { get; private set; }

    private void Awake()
    {
        ui_PlayerFaction = transform.Find("UI_PlayerFaction").GetComponent<TextMeshProUGUI>();
        ui_EnemyFaction = transform.Find("UI_EnemyFaction").GetComponent<TextMeshProUGUI>();
        ui_Kills = transform.Find("UI_Kills").GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        ui_PlayerFaction.text = "" + GameManager.Instance.playerFactions.Count();
        ui_EnemyFaction.text = "" + GameManager.Instance.enemyFactions.Count();
        ui_Kills.text = "" + GameManager.Instance.kills;
    }
}
