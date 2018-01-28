using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private float CommandInterval = 1;
    [SerializeField]
    private float CommandDelay = 5;
    [SerializeField]
    private float CommandDuration = 1;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private Rover[] roverPrefabs;
    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private Sprite[] playerGunsSprites;

    [SerializeField]
    private UICommandQueue[] playerCommandQueueUI;

    [SerializeField]
    private Material[] laserColor;

    [SerializeField]
    private AudioEvent audioEventSendCommand;
    [SerializeField]
    private AudioEvent audioEventReceiveCommand;

    private List<Player> players;
    private List<Rover> rovers;
    private List<LinkedList<PlayerCommand>> playerCommandQueues;

    private float lastCommand;

    private void Start()
    {
        if (players == null)
        {
            // players was null, so assume we are in a test scene and create our own players.
            players = new List<Player>();
            const int nPlayers = 2;
            for (int i = 0; i < nPlayers; i++)
            {
                Player player = Player.Create(i);
                players.Add(player);
            }

            Initialise(players);
        }
    }

    public void Initialise(List<Player> players)
    {
        this.players = players;
        int nPlayers = players.Count;
        rovers = new List<Rover>(nPlayers);
        playerCommandQueues = new List<LinkedList<PlayerCommand>>(nPlayers);

        List<Transform> cameraTargets = new List<Transform>();

        for (int i = 0; i < nPlayers; i++)
        {
            LinkedList<PlayerCommand> queue = new LinkedList<PlayerCommand>();
            playerCommandQueues.Add(queue);

            Rover rover = Instantiate<Rover>(roverPrefabs[i]);
            rover.Initialise(i, i, playerSprites[i], playerGunsSprites[i], laserColor[i]);
            rover.transform.position = spawnPositions[i].position;
            rovers.Add(rover);

            cameraTargets.Add(rover.transform);

            Assert.IsNotNull(playerCommandQueueUI[i]);
            playerCommandQueueUI[i].TotalDelay = CommandDelay;
        }

        TopDownCamera2D cam = Camera.main.GetComponent<TopDownCamera2D>();
        cam.Targets = cameraTargets;

        lastCommand = Time.time;
    }

    private void Update()
    {
        Assert.IsNotNull(players);
        Assert.IsNotNull(rovers);
        Assert.IsNotNull(playerCommandQueues);

        GetPlayerCommandsInput();
        ProcessCommands();
    }

    private void GetPlayerCommandsInput()
    {
        if(GamepadInput.GamePad.GetButton(GamepadInput.GamePad.Button.Start, GamepadInput.GamePad.Index.Any))
        {
            Reset();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

        // For each player, get command input, if any, and put it into the command queue.
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++)
        {
            PlayerCommand command = players[playerIndex].GetNextCommand();
            if (command != null)
            {
                // Set sent time, and put the command in the queue.
                LinkedList<PlayerCommand> queue = playerCommandQueues[playerIndex];
                Assert.IsNotNull(queue);

                float sentTime = Mathf.Ceil(Time.time);
                if (queue.Count > 0)
                {
                    // Make sure we have an interval between commands.
                    PlayerCommand prevCommand = queue.Last.Value;
                    sentTime = Mathf.Max(sentTime, prevCommand.SentTime + CommandInterval);
                }
                command.SentTime = sentTime;
                command.Delay = CommandDelay;
                queue.AddLast(command);

                UICommandQueue uiQueue = playerCommandQueueUI[playerIndex];
                Assert.IsNotNull(uiQueue);
                uiQueue.Add(command);

                AudioManager.PlayAtIndex(audioEventSendCommand, (int)command.Action);
            }
        }
    }

    private void Reset()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    private void ProcessCommands()
    {
        // If it's time to execute commands
        if (Time.time > lastCommand + CommandInterval)
        {
            lastCommand = Time.time;

            // for each player's command queue
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++)
            {
                // take the next command from the queue
                LinkedList<PlayerCommand> queue = playerCommandQueues[playerIndex];
                ProcessCommandQueue(queue);
            }
        }
    }

    private void ProcessCommandQueue(LinkedList<PlayerCommand> queue)
    {
        Assert.IsNotNull(queue);
        if(queue.Count > 0)
        {
            PlayerCommand command = queue.First.Value;

            // if the command can be received now, and we are not already executing a command
            if (command != null && Time.time > command.SentTime + CommandDelay)
            {
                // dequeue and execute the commmand
                queue.RemoveFirst();
                Assert.IsNotNull(rovers[command.RoverId]);
                rovers[command.RoverId].EnqueueCommand(command, CommandDuration);

                UICommandQueue uiQueue = playerCommandQueueUI[command.PlayerId];
                Assert.IsNotNull(uiQueue);
                uiQueue.Remove(command);
            }
        }
    }
}
