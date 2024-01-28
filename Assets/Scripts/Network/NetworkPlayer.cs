using Mirror;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkConnectionToClient Conn { get; private set; }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        SceneManager.LoadScene("scenes/Playerscene");
    }

    public void Init(NetworkConnectionToClient conn)
    {
        Conn = conn;
    }
}
