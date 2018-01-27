using GamepadInput;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId { get; set; }

    private const float deadZone = 0.2f;

    private Queue<ActionType> inputQueue = new Queue<ActionType>();
    private ActionType prevInput;

    public static Player Create(int playerId)
    {
        string objName = string.Format("Player{0}", playerId);
        GameObject obj = new GameObject(objName);
        Player player = obj.AddComponent<Player>();
        player.Initialise(playerId);
        return player;
    }

    public void Initialise(int playerId)
    {
        PlayerId = playerId;
    }

    private void Update()
    {
        GamePad.Index controllerIndex = (GamePad.Index)(PlayerId + 1);
        Vector2 leftStick = GamePad.GetAxis(GamePad.Axis.LeftStick, controllerIndex);
        bool shoot = GamePad.GetButton(GamePad.Button.A, controllerIndex)
                  || GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, controllerIndex) > deadZone
                  || GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, controllerIndex) > deadZone;

        ActionType newInput = ActionType.None;

        if (leftStick.y > 0 + deadZone)
            newInput = ActionType.Forward;
        else if (leftStick.y < 0 - deadZone)
            newInput = ActionType.Reverse;
        else if (leftStick.x > 0 + deadZone)
            newInput = ActionType.RotateRight;
        else if (leftStick.x < 0 - deadZone)
            newInput = ActionType.RotateLeft;
        else if (shoot)
            newInput = ActionType.Shoot;

        if(newInput != prevInput && newInput != ActionType.None)
        {
            inputQueue.Enqueue(newInput);
        }
        prevInput = newInput;
    }

    public PlayerCommand GetNextCommand()
    {
        if(inputQueue.Count > 0)
        {
            ActionType action = inputQueue.Dequeue();
            int roverId = PlayerId;
            PlayerCommand command = new PlayerCommand(PlayerId, roverId, action);
            return command;
        }
        else
            return null;
    }
}
