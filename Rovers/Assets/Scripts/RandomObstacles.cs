using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns an object in random positions.
public class RandomObstacles : MonoBehaviour
{
    [SerializeField]
    private int objectCount;

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

            Decoration obj = Instantiate(prefab, transform);
            obj.transform.position = new Vector2(x, y);

            obj.SelectRandomRotation();
            obj.SelectRandomSprite();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(area.center, area.size);
    }
}
