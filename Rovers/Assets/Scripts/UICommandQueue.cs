using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UICommandQueue : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeline;

    [SerializeField]
    private UICommand commandPrefab;

    [SerializeField]
    private Sprite[] actionIcons;

    public float TotalDelay { get; set; }

    private Dictionary<PlayerCommand, UICommand> commandObjects = new Dictionary<PlayerCommand, UICommand>();

    private void Update()
    {
        float timelineHeight = timeline.rect.height;

        foreach(var pair in commandObjects)
        {
            PlayerCommand command = pair.Key;
            UICommand obj = pair.Value;
            float scheduled = command.SentTime + command.Delay;
            // t=1 -> just sent
            // t=0 -> being received
            float durationRemaining = scheduled - Time.time;
            float t = Mathf.Clamp01(durationRemaining / TotalDelay);

            RectTransform tr = obj.GetComponent<RectTransform>();
            tr.anchoredPosition = new Vector2(0, t * timelineHeight);
        }
    }

    public void Add(PlayerCommand command)
    {
        UICommand commandObj = Instantiate(commandPrefab);
        RectTransform tr = commandObj.GetComponent<RectTransform>();
        tr.SetParent(timeline);
        commandObjects.Add(command, commandObj);

        Sprite icon = actionIcons[(int)command.Action];
        Assert.IsNotNull(icon);
        commandObj.SetIcon(icon);
    }

    public void Remove(PlayerCommand command)
    {
        UICommand commandObj = commandObjects[command];
        Destroy(commandObj.gameObject);
        commandObjects.Remove(command);
    }
}
