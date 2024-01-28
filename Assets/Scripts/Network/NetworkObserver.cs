using Mirror;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        base.OnStartLocalPlayer();

        SceneManager.LoadScene("scenes/StageEditorScene");
        Destroy(UIManager.Instance.gameObject);
    }

    [Command]
    public void CmdSendLevel(List<int> a, List<int> b)
    {
        NetworkPlayer player = NetManager.singleton!.Player;
        if (player == null)
            return;

       //player.SetLevel(level);
    }

    [ClientRpc]
    public void SetSubmittedLevelInteractable(bool interactable)
    {

    }

    [Command]
    public void CmdInteract(int toInteract)
    {

    }
}
