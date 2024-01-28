using Mirror;

public class NetworkObserver : NetworkBehaviour
{
    public NetworkConnectionToClient Conn { get; private set; }

    public void Init(NetworkConnectionToClient conn, string newName)
    {
        name = newName;
        Conn = conn;
    }

    public override void OnStartLocalPlayer()
    {
        // Load 
    }

    [ClientRpc]
    public void SetSubmittedLevelInteractable(bool interactable)
    {

    }

    [Command]
    public void Interact(int toInteract)
    {

    }
}
