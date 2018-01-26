using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand
{
    public int PlayerId { get; private set; }
    public int RoverId { get; private set; }
    public ActionType Action { get; private set; }
    public float SentTime { get; private set; }

    public PlayerCommand(int playerId, int roverId, ActionType type, float sentTime)
    {
        PlayerId = playerId;
        RoverId = roverId;
        Action = type;
        SentTime = sentTime;
    }
}
