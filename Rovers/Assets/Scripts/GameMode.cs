using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private float CommandInterval = 1;
    [SerializeField]
    private float CommandDelay = 5;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private Rover[] roverPrefabs;

    private List<Player> players;
    private List<Rover> rovers;
    private List<Queue<PlayerCommand>> playerCommandQueues;

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
        playerCommandQueues = new List<Queue<PlayerCommand>>(nPlayers);

        for (int i = 0; i < nPlayers; i++)
        {
            Queue<PlayerCommand> queue = new Queue<PlayerCommand>();
            playerCommandQueues.Add(queue);

            Rover rover = Instantiate<Rover>(roverPrefabs[i]);
            rover.Initialise(i, i);
            rover.transform.position = spawnPositions[i].position;
            rovers.Add(rover);
        }

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
        // For each player, get command input, if any, and put it into the command queue.
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++)
        {
            PlayerCommand command = players[playerIndex].GetCommand();
            if (command != null)
            {
                // Set sent time, and put the command in the queue.
                command.SentTime = Mathf.Ceil(Time.time);
                Queue<PlayerCommand> queue = playerCommandQueues[playerIndex];
                Assert.IsNotNull(queue);
                queue.Enqueue(command);
            }
        }
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
                Queue<PlayerCommand> queue = playerCommandQueues[playerIndex];
                ProcessCommandQueue(queue);
            }
        }
    }

    private void ProcessCommandQueue(Queue<PlayerCommand> queue)
    {
        Assert.IsNotNull(queue);
        if(queue.Count > 0)
        {
            PlayerCommand command = queue.Peek();

            // if the command can be received now, and we are not already executing a command
            if (command != null && Time.time > command.SentTime + CommandDelay && !rovers[command.RoverId].IsExecutingCommand)
            {
                // dequeue and execute the commmand
                queue.Dequeue();
                Assert.IsNotNull(rovers[command.RoverId]);
                rovers[command.RoverId].ExecuteCommand(command);
            }
        }
    }
}
