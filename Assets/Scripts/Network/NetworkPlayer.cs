using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkConnectionToClient Conn { get; private set; }

    public void Init(NetworkConnectionToClient conn)
    {
        Conn = conn;
    }
}
