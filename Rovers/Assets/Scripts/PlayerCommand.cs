public class PlayerCommand
{
    public int PlayerId { get; private set; }
    public int RoverId { get; private set; }
    public ActionType Action { get; private set; }
    public float SentTime { get; set; }
    public float Delay { get; set; }

    public PlayerCommand(int playerId, int roverId, ActionType type)
    {
        PlayerId = playerId;
        RoverId = roverId;
        Action = type;
    }
}
