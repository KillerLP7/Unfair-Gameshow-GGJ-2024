using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchList : MonoBehaviour
{
    [SerializeField] private TMP_InputField ownName;
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private TMP_InputField port;

    [SerializeField] private Button cancelButton;

    private ExtendedCoroutine connectedRoutine;

    private void Start()
    {
#if UNITY_WEBGL
        NetworkClient.OnConnectedEvent += OnConnected;
#else
        HostMatch();
#endif
    }

    public void OnTryToConnect()
    {
        if (connectedRoutine != null)
            return;

        cancelButton.interactable = false;
        connectedRoutine = new ExtendedCoroutine(this, Connecting(), startNow: true);
    }

    public void StopTryingToConnect()
    {
        StopCoroutine(connectedRoutine.Coroutine);
        connectedRoutine = null;
        cancelButton.interactable = true;

        NetManager.singleton.StopClient();
    }

    private void OnConnected()
    {
        StopTryingToConnect();
        Destroy(gameObject);
    }

    private IEnumerator Connecting()
    {
        yield return new WaitForSeconds(7.0f);

        if (!NetworkClient.isConnected)
            StopTryingToConnect();
    }

    public void HostMatch()
    {
        NetManager.singleton.StartHost();
        Destroy(gameObject);
    }

}
