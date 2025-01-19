using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MouseCrosshair : MonoBehaviour
{
    private Image crosshair;

    private void Awake()
    {
        crosshair = GetComponent<Image>();
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
