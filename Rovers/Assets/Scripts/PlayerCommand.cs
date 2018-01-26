using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand
{
    public ActionType Action { get; private set; }
    public float SentTime { get; private set; }

    public PlayerCommand(ActionType type, float sentTime)
    {
        Action = type;
        SentTime = sentTime;
    }
}
