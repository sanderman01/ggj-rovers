using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Decoration : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sprites;

    private void Awake()
    {
        if(!Application.isPlaying)
        {
            SelectRandomSprite();
            SelectRandomRotation();
        }
    }

    public void SelectRandomSprite()
    {
        int index = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[index];
    }

    public void SelectRandomRotation()
    {
        float angle = Random.value * 360f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
