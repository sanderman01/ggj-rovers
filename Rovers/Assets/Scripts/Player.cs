using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId { get; set; }

    public static Player Create(int playerId)
    {
        string objName = string.Format("Player{0}", playerId);
        GameObject obj = new GameObject(objName);
        Player player = obj.AddComponent<Player>();
        player.Initialise(playerId);
        return player;
    }

    public void Initialise(int playerId)
    {
        PlayerId = playerId;
    }

    public PlayerCommand GetCommand()
    {
        // TODO
        return null;
    }
}
