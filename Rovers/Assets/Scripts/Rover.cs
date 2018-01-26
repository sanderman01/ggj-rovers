using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public int PlayerId { get; set; }
    public int RoverId { get; set; }

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
				StartCoroutine(MoveForward());
                break;
            case ActionType.Reverse:
				StartCoroutine(MoveBackward());
				break;
            case ActionType.RotateLeft:
				StartCoroutine(RotateLeft());
				break;
            case ActionType.RotateRight:
				StartCoroutine(RotateRight());
				break;
            case ActionType.Shoot:
				StartCoroutine(Shoot());
				break;
            case ActionType.RotateTurretLeft:
				StartCoroutine(RotateTurretLeft());
				break;
            case ActionType.RotateTurretRight:
				StartCoroutine(RotateTurretRight());
				break;
            default:
                throw new System.NotImplementedException();
        }
    }
	private IEnumerator MoveForward()
	{
		yield return null;
	}

	private IEnumerator MoveBackward()
	{
		yield return null;
	}

	private IEnumerator RotateLeft()
	{
		yield return null;
	}

	private IEnumerator RotateRight()
	{
		yield return null;
	}

	private IEnumerator Shoot()
	{
		yield return null;
	}

	private IEnumerator RotateTurretLeft()
	{
		yield return null;
	}

	private IEnumerator RotateTurretRight()
	{
		yield return null;
	}
}
