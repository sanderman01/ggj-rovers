using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
	public int PlayerId { get; set; }
	public int RoverId { get; set; }
	public int MoveSpeed { get; set; }
    public int TurnSpeed { get; set; }

	public bool IsExecutingCommand { get; private set; }

	private Rigidbody rigidBody;

    public void Initialise(int playerId, int roverId)
    {
        PlayerId = playerId;
        RoverId = roverId;
    }

	private void Awake()
	{
        MoveSpeed = 3;
        TurnSpeed = 2;
        rigidBody = GetComponent<Rigidbody>();
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

    public void MoveForward2()
    {
        StartCoroutine(MoveForward());
    }

    public void MoveBackward2()
    {
        StartCoroutine(MoveBackward());
    }

    public void RotateLeft2()
    {
        StartCoroutine(RotateLeft());
    }

    public void RotateRight2()
    {
        StartCoroutine(RotateRight());
    }

    private IEnumerator MoveForward()
	{
        // Create a vector in the direction the tank is facing with a magnitude based on speed and the time between frames.
        Vector3 movement = transform.up * MoveSpeed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        rigidBody.MovePosition(rigidBody.position + movement);
        yield return null;
	}

	private IEnumerator MoveBackward()
	{
        // Create a vector in the direction the tank is facing with a magnitude based on speed and the time between frames.
        Vector3 movement = -transform.up * MoveSpeed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        rigidBody.MovePosition(rigidBody.position + movement);
        yield return null;
	}

	private IEnumerator RotateLeft()
	{
        // Determine the number of degrees to be turned based on the speed and time between frames.
        float turn = TurnSpeed * Time.deltaTime;
        Debug.Log("left"+turn);

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, 0f, turn);

        // Apply this rotation to the rigidbody's rotation.
        rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
        yield return null;
	}

	private IEnumerator RotateRight()
	{
        int totalturning = 0;
        for(int i=0; i<= 45;i++)
        {
            // Determine the number of degrees to be turned based on the speed and time between frames.
            Debug.Log("turn: "+TurnSpeed+" total: "+totalturning);
            int turn = TurnSpeed * 1;
            if(totalturning + turn > 90)
            {
                turn = 90 - totalturning;
            }

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler(0f, 0f, -turn);

            // Apply this rotation to the rigidbody's rotation.
            rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
            totalturning += turn;
            yield return null;
        }
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
