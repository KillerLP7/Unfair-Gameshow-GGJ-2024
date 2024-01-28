using Mirror;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkConnectionToClient Conn { get; private set; }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        SceneManager.LoadScene("scenes/Playerscene", LoadSceneMode.Additive);
    }

    public void Init(NetworkConnectionToClient conn)
    {
        Conn = conn;
    }

    [TargetRpc]
    public void SetLevel(List<int> a, List<int> b, uint id)
    {
        if (!isLocalPlayer) return;

        Dictionary<int, int> c = new Dictionary<int, int>();
        for (int i = 0; i < a.Count; i++)
        {
            c[a[i]] = b[i];
        }

        GameManager.Instance.SetLevel(c, id);
    }

    [TargetRpc]
    public void Interact(int toInteract, uint netID)
    {
        if (!isLocalPlayer) return;
        GameManager.Instance.ObserverInteractWithLevel(netID, toInteract);

    }

    [Command]
    public void CmdLevelLoaded(uint pObserverID, int amountOfStuffToInteractWith)
    {
        NetworkObserver guy = null;
        foreach (NetworkObserver item in NetManager.singleton.Observers)
        {
            if (item.netIdentity.netId == pObserverID)
                guy = item;
        }
        if (guy == null)
        {
            print("ffffffff");
            return;
        }

        guy.SetSubmittedLevelInteractable(amountOfStuffToInteractWith);
    }

    [Command]
    public void CmdLevelFinshed(uint pObserverID)
    {
        NetworkObserver guy = null;
        foreach (NetworkObserver item in NetManager.singleton.Observers)
        {
            if (item.netIdentity.netId == pObserverID)
                guy = item;
        }
        if (guy == null)
        {
            print("ffffffff");
            return;
        }

        guy.SubmittedLevelFinished();
    }
}
