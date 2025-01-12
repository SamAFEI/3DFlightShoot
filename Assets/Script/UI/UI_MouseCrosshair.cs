using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UI_MouseCrosshair : MonoBehaviour
{
    private Image locked;
    private Image crosshair;
    public GameObject uiHealth { get; private set; }
    public LayerMask enemyLayerMask;
    public ShipController shipController;
    public GameObject targetObj;

    private void Awake()
    {
        crosshair = GetComponent<Image>();
        uiHealth = transform.Find("UI_Health").gameObject;
    }

    private void Update()
    {
        if (crosshair != null)
        {
            Vector2 cursor = Input.mousePosition;
            crosshair.transform.position = cursor;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
