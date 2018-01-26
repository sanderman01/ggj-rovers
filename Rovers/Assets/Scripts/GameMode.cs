using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private List<Player> players;
    private List<Rover> rovers;
    private SortedList<float, PlayerCommand> commandQueue;

    private void Update()
    {
        // For each player, get command input, if any, and put it into the command queue.

        // If it's time to execute commands, then:
        // For each command in queue where (sentTime + currentDelay) < currentTime
        // And the rover is not already executing a command
        //     Then give the command to the rover.
    }
}
