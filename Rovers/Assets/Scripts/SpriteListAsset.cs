using UnityEngine;

public class SpriteListAsset : MonoBehaviour
{
    public Sprite[] Items { get { return list; } }

    [SerializeField]
    private Sprite[] list;
}
