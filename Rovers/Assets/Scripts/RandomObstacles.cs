using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns an object in random positions.
public class RandomObstacles : MonoBehaviour
{
    [SerializeField]
    private int objectCount;

    [SerializeField]
    private bool randomRotate = true;
    [SerializeField]
    private bool randomSprite = true;

    [SerializeField]
    private Decoration prefab;

    [SerializeField]
    private Rect area;

    // Use this for initialization
    void Start()
    {
        int xMin = Mathf.FloorToInt(area.xMin);
        int yMin = Mathf.FloorToInt(area.yMin);
        int xMax = Mathf.FloorToInt(area.xMax);
        int yMax = Mathf.FloorToInt(area.yMax);

        for (int i = 0; i < objectCount; i++)
        {
            int x = Random.Range(xMin, xMax);
            int y = Random.Range(yMin, yMax);
            Vector2 pos = new Vector2(x, y);

            // Check if the position is occupied, if so, skip this one.
            Collider2D collider = Physics2D.OverlapPoint(pos);
            if (collider == null)
            {
                Decoration obj = Instantiate(prefab, transform);
                obj.transform.position = new Vector2(x, y);

                if (randomRotate) obj.SelectRandomRotation();
                if (randomSprite) obj.SelectRandomSprite();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(area.center, area.size);
    }
}
