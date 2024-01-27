using Mirror.SimpleWeb;
using UnityEngine;
using static NetJoin;

public class MatchDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI matchName;
    private Match match;

    public void SetMatch(Match match)
    {
        this.match = match;
        matchName.text = match.name;
    }

    public void JoinMatch()
    {
        NetManager.singleton.networkAddress = match.ip;
        (NetManager.singleton.transport as SimpleWebTransport).port = match.port;
        NetManager.singleton.StartClient();
    }
}
