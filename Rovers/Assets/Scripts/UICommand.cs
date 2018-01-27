using UnityEngine;
using UnityEngine.UI;

public class UICommand : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image icon;

    public void SetColor(Color color)
    {
        // TODO
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
