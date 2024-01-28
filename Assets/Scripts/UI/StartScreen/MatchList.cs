using Mirror;
using Mirror.SimpleWeb;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
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
        NetManager.singleton.connected.AddListener(OnConnected);

#if UNITY_WEBGL

#elif UNITY_SERVER
        HostServer();
#else
        HostMatch();
#endif
    }

    private void OnDestroy()
    {
        NetManager.singleton.connected.RemoveListener(OnConnected);
    }

    public void OnTryToConnect()
    {
        if (connectedRoutine != null)
            return;

        cancelButton.interactable = false;
        connectedRoutine = new ExtendedCoroutine(this, Connecting(), startNow: true);
    }

    public void StopTryingToConnect(bool stop = true)
    {
        StopCoroutine(connectedRoutine.Coroutine);
        connectedRoutine = null;
        cancelButton.interactable = true;

        if (stop)
            NetManager.singleton.StopClient();
    }

    private void OnConnected()
    {
        print("Connected!");
        StopTryingToConnect(false);
        Destroy(gameObject);
    }

    private IEnumerator Connecting()
    {
        yield return null; // ignore this, dont delete it

        if (!ushort.TryParse(port.text, out ushort castedPort) || !ValidAddress(ip.text))
        {
            StopTryingToConnect();
            yield break;
        }

        (NetManager.singleton.transport as SimpleWebTransport).port = castedPort;
        NetManager.singleton.networkAddress = ip.text;
        NetManager.singleton.StartClient();

        yield return new WaitForSeconds(7.0f);

        StopTryingToConnect();
    }

    private bool ValidAddress(string address)
    {
        // First, try to parse the address as an IP address.
        if (IPAddress.TryParse(address, out _))
        {
            return true; // It's a valid IP address
        }

        // If it's not an IP address, check if it's a valid hostname
        // Simple regex for a basic hostname validation (you might need a more complex one depending on your requirements)
        string hostnameRegex = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
        return Regex.IsMatch(address, hostnameRegex);
    }

    public void HostServer()
    {
        NetManager.singleton.StartServer();
        Destroy(gameObject);
    }

    public void HostMatch()
    {
        NetManager.singleton.StartHost();
        Destroy(gameObject);
    }

}
