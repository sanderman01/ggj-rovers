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
        bool forward = false;
        bool left = false;
        bool right = false;
        bool shoot = false;
        bool reverse = false;

        // Key input
        KeyCode keyForward = KeyCode.UpArrow;
        KeyCode keyLeft = KeyCode.LeftArrow;
        KeyCode keyRight = KeyCode.RightArrow;
        KeyCode keyShoot = KeyCode.DownArrow;

        switch (PlayerId)
        {
            case 0:
                keyForward = KeyCode.W;
                keyLeft = KeyCode.A;
                keyRight = KeyCode.D;
                keyShoot = KeyCode.S;
                break;
            case 1:
                keyForward = KeyCode.UpArrow;
                keyLeft = KeyCode.LeftArrow;
                keyRight = KeyCode.RightArrow;
                keyShoot = KeyCode.DownArrow;
                break;
        }

        forward |= Input.GetKeyDown(keyForward);
        right |= Input.GetKeyDown(keyRight);
        left |= Input.GetKeyDown(keyLeft);
        shoot |= Input.GetKeyDown(keyShoot);

        // Gamepad input
        GamePad.Index controllerIndex = (GamePad.Index)(PlayerId + 1);
        Vector2 leftStick = GamePad.GetAxis(GamePad.Axis.LeftStick, controllerIndex);
        shoot |= GamePad.GetButton(GamePad.Button.A, controllerIndex)
                  || GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, controllerIndex) > deadZone
                  || GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, controllerIndex) > deadZone;

        forward |= leftStick.y > 0 + deadZone;
        reverse |= leftStick.y < 0 - deadZone;
        right |= leftStick.x > 0 + deadZone;
        left |= leftStick.x < 0 - deadZone;

        // Translate to commands
        ActionType newInput = ActionType.None;

        if (forward)
            newInput = ActionType.Forward;
        else if (reverse)  
            newInput = ActionType.None; // This disables reverse for now.
        else if (right)
            newInput = ActionType.RotateRight;
        else if (left)
            newInput = ActionType.RotateLeft;
        else if (shoot)
            newInput = ActionType.Shoot;

        if (newInput != prevInput && newInput != ActionType.None)
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
