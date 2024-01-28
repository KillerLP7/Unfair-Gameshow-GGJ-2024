using Mirror;

public struct HelloMessage : NetworkMessage
{
    public bool wantsToBePlayer;
    public string name;
}
