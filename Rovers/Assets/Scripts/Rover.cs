using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public int PlayerId { get; set; }
    public int RoverId { get; set; }
    public int MoveSpeed { get; set; }
    public int MovementDistance { get; set; }
    public float DefaultCommandDuration { get; set; }
    public bool IsExecutingCommand { get; private set; }

    private Rigidbody rigidBody;

    private Queue<PlayerCommand> commandQueue = new Queue<PlayerCommand>();

    public void Initialise(int playerId, int roverId, Sprite sprite)
    {
        PlayerId = playerId;
        RoverId = roverId;
        SetTexture(sprite);
    }

    private void SetTexture(Sprite sprite)
    {
        SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    private void Awake()
    {
        MovementDistance = 5;
        DefaultCommandDuration = 5;
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
        DefaultCommandDuration = standardCommandDuration;
        commandQueue.Enqueue(command);
    }

    private void ExecuteCommand(PlayerCommand command)
    {
        switch (command.Action)
        {
            case ActionType.Forward:
                StartCoroutine(Move(transform.up));
                break;
            case ActionType.Reverse:
                StartCoroutine(Move(-transform.up));
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

    private IEnumerator Move(Vector3 direction)
    {
        IsExecutingCommand = true;
        float beginTime = Time.time;
        float endTime = beginTime + DefaultCommandDuration;
        float duration = endTime - beginTime;

        Vector3 beginLoc = transform.position;
        Vector3 movement = direction * MovementDistance;
        Vector3 endLoc = transform.position + movement;

        while (Time.time < endTime)
        {
            float t = (Time.time - beginTime) / duration;
            Vector3 currentLocation = beginLoc + (movement * t);
            rigidBody.position = currentLocation;
            yield return null;
        }
        rigidBody.position = endLoc;
        IsExecutingCommand = false;
    }

    private IEnumerator Rotate(float angle)
    {
        IsExecutingCommand = true;
        float beginTime = Time.time;
        float endTime = beginTime + DefaultCommandDuration;
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
}
