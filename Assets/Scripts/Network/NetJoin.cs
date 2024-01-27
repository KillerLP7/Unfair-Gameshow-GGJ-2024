using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class NetJoin : MonoBehaviour
{
    public class Match
    {
        public string name;
        public string ip;
        public ushort port;

        public Match(string name, string ip, ushort port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }
    }

    public IEnumerator HostMatch(UnityAction<bool> onFinished)
    {       
        print(NetManager.singleton.networkAddress);
        SimpleWebTransport trans = NetManager.singleton.transport as SimpleWebTransport; ;

        string ip = GetLocalIPAddress();
        int port = trans.port;
        string name = "Test";

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = name;
        data["ip"] = ip;
        data["port"] = port.ToString();

        using UnityWebRequest request = UnityWebRequest.Post("http://rhyth.de:9025/match", data);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
            onFinished.Invoke(false);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            onFinished.Invoke(true);
        }
    }

    public void DeleteMatch()
    {

    }

    public IEnumerator GetAllMatches(UnityAction<List<Match>> onGottenMatches)
    {
        using UnityWebRequest request = UnityWebRequest.Get("http://rhyth.de:9025/match");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
            onGottenMatches.Invoke(new List<Match>());
        }

        Debug.Log(request.downloadHandler.text);
        List<Match> toReturnMatches = new();

        string[] matches = request.downloadHandler.text.Split('\n');
        foreach (string match in matches)
        {
            string[] args = match.Split(',');
            if (args.Length != 3)
                continue;

            string name = args[0];
            string ip = args[1];
            ushort port = ushort.Parse(args[2]);

            Match m = new Match(name, ip, port);
            Debug.Log(m.name + " " + m.ip + " " + m.port);

            toReturnMatches.Add(m);
        }

        onGottenMatches.Invoke(toReturnMatches);
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
