using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    public Color flashColor;
    private Color color;
    public Image image;

    private void Start()
    {
        color = image.color;
    }

    public void DoFlash(bool value)
    {
        if (value)
        {
            image.color = flashColor;
        }
        else
        {
            image.color = color;
        }
    }
}
