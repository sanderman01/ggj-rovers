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
    private LineRenderer laserLine;
    private Rigidbody2D rigidBody;
    [SerializeField]
    private SpriteRenderer vehicleRenderer;
    [SerializeField]
    private SpriteRenderer turretRenderer;

    private Queue<PlayerCommand> commandQueue = new Queue<PlayerCommand>();

    public void Initialise(int playerId, int roverId, Sprite vehicle, Sprite turret, Material lasercolor)
    {
        PlayerId = playerId;
        RoverId = roverId;
        SetTexture(vehicle, vehicleRenderer);
        SetTexture(turret, turretRenderer);
        SetLaserColor(lasercolor);
    }

    private void SetTexture(Sprite sprite, SpriteRenderer renderer)
    {
        renderer.sprite = sprite;
    }
    private void SetLaserColor(Material color)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.material = color;
    }

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        MovementDistance = 5;
        DefaultCommandDuration = 5;
        rigidBody = GetComponent<Rigidbody2D>();
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

    private IEnumerator Move(Vector2 direction)
    {
        IsExecutingCommand = true;
        float beginTime = Time.time;
        float endTime = beginTime + DefaultCommandDuration;
        float duration = endTime - beginTime;

        Vector2 beginLoc = transform.position;
        Vector2 movement = direction * MovementDistance;
        Vector2 endLoc = beginLoc + movement;

        while (Time.time < endTime)
        {
            float t = (Time.time - beginTime) / duration;
            Vector2 currentLocation = beginLoc + (movement * t);
            rigidBody.transform.position = currentLocation;
            yield return null;
        }
        rigidBody.transform.position = endLoc;
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
        IsExecutingCommand = true;
        laserLine.enabled = true;
        var up = Vector3.up;
        var length = up * 100;
        laserLine.SetPosition(0, length); 
        RaycastHit hit;

        //Debug.DrawRay(transform.position, -up * 2, Color.green);

        if (Physics.Raycast(transform.position, length, out hit, 2))
        {

            Debug.Log("HIT");

        //    //if (hit.collider.gameObject.name == "rover")
        //    //{
        //    //    Destroy(GetComponent("Rigidbody"));
        //    //}
        }
        yield return new WaitForSeconds(DefaultCommandDuration);
        laserLine.enabled = false;
        IsExecutingCommand = false;
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
