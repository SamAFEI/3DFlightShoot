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
            crosshair.transform.position = Input.mousePosition;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
