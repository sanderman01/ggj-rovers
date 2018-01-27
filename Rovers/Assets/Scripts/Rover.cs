using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public int PlayerId { get; set; }
    public int RoverId { get; set; }
    public int MoveSpeed { get; set; }
    public int TurnSpeed { get; set; }
    public float defaultCommandDuration { get; set; }
    public bool IsExecutingCommand { get; private set; }

    private bool rotating;

    private Rigidbody rigidBody;

    private Queue<PlayerCommand> commandQueue = new Queue<PlayerCommand>();

    public void Initialise(int playerId, int roverId)
    {
        PlayerId = playerId;
        RoverId = roverId;
    }

    private void Awake()
    {
        MoveSpeed = 3;
        TurnSpeed = 2;
        rotating = false;
        defaultCommandDuration = 5;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!IsExecutingCommand && commandQueue.Count > 0)
        {
            PlayerCommand command = commandQueue.Dequeue();
            ExecuteCommand(command);
        }
    }

    public void EnqueueCommand(PlayerCommand command, float standardCommandDuration)
    {
        defaultCommandDuration = standardCommandDuration;
        commandQueue.Enqueue(command);
    }

    private void ExecuteCommand(PlayerCommand command)
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
                StartCoroutine(Rotate(90));
                break;
            case ActionType.RotateRight:
                StartCoroutine(Rotate(-90));
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
        StartCoroutine(Rotate(90));
    }

    public void RotateRight2()
    {
        StartCoroutine(Rotate(-90));
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

    private IEnumerator Rotate(float angle)
    {
        IsExecutingCommand = true;
        float beginTime = Time.time;
        float endTime = beginTime + defaultCommandDuration;
        float duration = endTime - beginTime;

        Quaternion beginRot = transform.rotation;
        Quaternion newRot = beginRot * Quaternion.Euler(new Vector3(0, 0, angle));

        while (Time.time < endTime)
        {
            float t = (Time.time - beginTime) / duration;
            transform.rotation = Quaternion.Lerp(beginRot, newRot, t);
            yield return null;
        }

        transform.rotation = newRot;
        IsExecutingCommand = false;
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

    private Quaternion Rotate(Vector3 rotation)
    {
        Transform to = transform;
        to.Rotate(rotation);
        return Quaternion.Lerp(transform.rotation, to.rotation, defaultCommandDuration * TurnSpeed);
    }
}
