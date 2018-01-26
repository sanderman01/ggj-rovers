using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public int PlayerId;
    public int RoverId;

    public bool IsExecutingCommand { get; private set; }

    public void Initialise(int playerId, int roverId)
    {
        PlayerId = playerId;
        RoverId = roverId;
    }

    public void ExecuteCommand(PlayerCommand command)
    {
        switch (command.Action)
        {
            case ActionType.Forward:
                // Start coroutine or other process
                break;
            case ActionType.Reverse:
                // Start coroutine or other process
                break;
            case ActionType.RotateLeft:
                // Start coroutine or other process
                break;
            case ActionType.RotateRight:
                // Start coroutine or other process
                break;
            case ActionType.Shoot:
                // Start coroutine or other process
                break;
            case ActionType.RotateTurretLeft:
                // Start coroutine or other process
                break;
            case ActionType.RotateTurretRight:
                // Start coroutine or other process
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}
