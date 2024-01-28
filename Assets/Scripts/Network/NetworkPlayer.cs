using Mirror;
using System.Collections.Generic;
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

    [ClientRpc]
    public void SetLevel(List<int> a, List<int> b)
    {
        Dictionary<int, int> c = new Dictionary<int, int>();
        for (int i = 0; i < a.Count; i++)
        {
            c[a[i]] = b[i];
        }

        GameManager.Instance.SetLevel(c);
    }

    [ClientRpc]
    public void Interact(int toInteract)
    {

    }
}
