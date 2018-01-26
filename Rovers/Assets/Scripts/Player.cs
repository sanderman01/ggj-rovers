using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId { get; set; }

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
