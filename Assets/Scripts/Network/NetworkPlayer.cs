using Mirror;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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

    [ClientRpc]
    public void SetLevel(List<int> a, List<int> b, int id)
    {
        Dictionary<int, int> c = new Dictionary<int, int>();
        for (int i = 0; i < a.Count; i++)
        {
            c[a[i]] = b[i];
        }

        GameManager.Instance.SetLevel(c, id);
    }

    [ClientRpc]
    public void Interact(int toInteract, int netID)
    {
        GameManager.Instance.ObserverInteractWithLevel(netID, toInteract);

    }

    [Command]
    public void CmdLevelLoaded(int pObserverID, int amountOfStuffToInteractWith)
    {
        NetworkObserver observer = NetManager.singleton.Observers.First(x => x.netId == pObserverID);
        if (observer == null) return;

        observer.SetSubmittedLevelInteractable(amountOfStuffToInteractWith);
    }

    [Command]
    public void CmdLevelFinshed(int pObserverID)
    {
        NetworkObserver observer = NetManager.singleton.Observers.First(x => x.netId == pObserverID);
        if (observer == null) return;

        observer.SubmittedLevelFinished();
    }
}
