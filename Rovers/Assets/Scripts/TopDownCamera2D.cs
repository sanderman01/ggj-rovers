using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TopDownCamera2D : MonoBehaviour
{
    [SerializeField]
    private float damping = 2.0f;
    [SerializeField]
    private float minimumHeight = 50;
    [SerializeField]
    private float boundsMargin = 5;

    [SerializeField]
    private Rect rect;

    public List<Transform> Targets { get; set; }

    private void Update()
    {
        Assert.IsNotNull(Targets);

        Vector2 targetAvgPosition = Vector2.zero;
        float xMin = float.MaxValue;
        float yMin = float.MaxValue;
        float xMax = float.MinValue;
        float yMax = float.MinValue;
        foreach (Transform tr in Targets)
        {
            Assert.IsNotNull(tr);

            Vector2 p = (Vector2)tr.position;
            targetAvgPosition += p;
            xMin = Mathf.Min(xMin, p.x);
            yMin = Mathf.Min(yMin, p.y);
            xMax = Mathf.Max(xMax, p.x);
            yMax = Mathf.Max(yMax, p.y);
        }

        rect = new Rect(xMin - boundsMargin, yMin - boundsMargin, xMax - xMin + boundsMargin * 2, yMax - yMin + boundsMargin * 2);
        Vector2 center = rect.center;

        //rect.height = Mathf.Max(rect.height, minimumHeight);
        //rect.center = center;

        const float screenRatio = 9f / 16f;
        float height = Mathf.Max(rect.height, minimumHeight, rect.width * screenRatio);

        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = height * 0.5f;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = Vector2.Lerp(currentPos, center, Time.time * damping);
        targetPos.z = currentPos.z;
        transform.position = targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(rect.center, new Vector3(rect.width, rect.height, 1));
    }
}
