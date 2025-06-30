using UnityEngine;
using UnityEngine.UI;

public class LinkIndicator : MonoBehaviour
{
    public Image image; // 또는 SpriteRenderer 등

    public void SetColor(Color color)
    {
        image.color = color;
    }
}
