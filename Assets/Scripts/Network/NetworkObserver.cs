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

        SceneManager.LoadScene("scenes/StageEditorScene", LoadSceneMode.Additive);
        Destroy(UIManager.Instance.gameObject);

        NetManager.singleton.LocalObserver = this;
    }

    [Command]
    public void CmdSendLevel(List<int> a, List<int> b)
    {
        SetLevel(a, b, Conn.identity.netId);
    }

    [Server]
    public void SetLevel(List<int> a, List<int> b, uint id)
    {
        NetworkPlayer player = NetManager.singleton!.Player;
        if (player == null)
            return;

        player.SetLevel(a, b, id);
    }

    [TargetRpc]
    public void SetSubmittedLevelInteractable(int amountOfButtons)
    {
        if (!isLocalPlayer) return;
        if (StagePartEditor.instance != null)
        {
            StagePartEditor.instance.SetManualActivationKeysActive(amountOfButtons);
        }
    }

    [TargetRpc]
    public void SubmittedLevelFinished()
    {
        if (!isLocalPlayer) return;
        if (StagePartEditor.instance != null)
        {
            StagePartEditor.instance.SetManualActivationKeysActive(0);
        }
    }

    [Command]
    public void CmdInteract(int toInteract)
    {
        NetworkPlayer player = NetManager.singleton!.Player;
        if (player == null)
            return;

        player.Interact(toInteract, Conn.identity.netId);
    }
}
