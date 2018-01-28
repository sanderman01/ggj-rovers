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
    public int ShootingDistance { get; set; }
    public float DefaultCommandDuration { get; set; }
    public bool IsExecutingCommand { get; private set; }
    public Vector2 originO;
    private LineRenderer laserLine;
    private Rigidbody2D rigidBody;

    [SerializeField]
    private SpriteRenderer vehicleRenderer;
    [SerializeField]
    private SpriteRenderer turretRenderer;

    [SerializeField]
    private AudioEvent audioEventReceiveCommand;
    [SerializeField]
    private AudioEvent audioEventShootLaser;

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
        MovementDistance = 1;
        DefaultCommandDuration = 5;
        ShootingDistance = 10;
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
        AudioManager.PlayAtIndex(audioEventReceiveCommand, (int)command.Action);

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
                StartCoroutine(Shoot(ShootingDistance));
                break;
            case ActionType.RotateTurretLeft:
                StartCoroutine(RotateTurretLeft());
                break;
            case ActionType.RotateTurretRight:
                StartCoroutine(RotateTurretRight());
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        Vector2 beginLoc = transform.position;
        Vector2 movement = direction * MovementDistance;
        Vector2 endLoc = beginLoc + movement;

        // check if the next space is occupied
        if(Physics2D.OverlapCircle(endLoc, 0.4f))
        {
            Debug.LogWarning("Movement Blocked!");
            yield break;
        }

        IsExecutingCommand = true;

        float beginTime = Time.time;
        float endTime = beginTime + DefaultCommandDuration;
        float duration = endTime - beginTime;

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

    private IEnumerator Shoot(float distance)
    {
        AudioManager.PlayRandom(audioEventShootLaser);

        IsExecutingCommand = true;

        RaycastHit2D[] hits;
        Vector2 localOrigin = new Vector2(0, 1);
        Vector2 worldOrigin = transform.TransformPoint(localOrigin);
        Vector2 worldDirection = transform.TransformDirection(localOrigin);

        Vector3 laserEnd = worldOrigin + worldDirection * distance;

        //Vector2 origin = new Vector2(transform.position.x, transform.position.y+1);
        originO = worldOrigin;
        //Debug.DrawRay(transform.position, -up * 2, Color.green);
        hits = Physics2D.RaycastAll(worldOrigin, worldDirection, distance);
        if (hits != null && hits.Length > 0)
        {
            RaycastHit2D hit = FindClosestHit(hits, transform.position);
            laserEnd = hit.transform.position;

            Rover rover = hit.transform.GetComponent<Rover>();
            if(rover != null)
            {
                Debug.LogWarning("We hit a rover!");
                // TODO
            }
        }

        laserLine.enabled = true;
        laserLine.SetPosition(0, worldOrigin);
        laserLine.SetPosition(1, laserEnd);

        yield return new WaitForSeconds(DefaultCommandDuration);
        laserLine.enabled = false;
        IsExecutingCommand = false;
    }

    private RaycastHit2D FindClosestHit(RaycastHit2D[] hits, Vector2 origin)
    {
        float minDistance = float.MaxValue;
        RaycastHit2D result = new RaycastHit2D();

        for(int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            float distance = Vector2.Distance(origin, hit.transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                result = hit;
            }
        }
        return result;
    }

    private IEnumerator RotateTurretLeft()
    {
        yield return null;
    }

    private IEnumerator RotateTurretRight()
    {
        yield return null;
    }

    private void OnDrawGizmos()
    {
        if(originO != Vector2.zero)
        {
            Vector2 direction = transform.TransformDirection(new Vector2(0, 1));
            Ray ray = new Ray(originO, (Vector3)direction);
            Gizmos.DrawRay(ray);
        }
      
    }
}
