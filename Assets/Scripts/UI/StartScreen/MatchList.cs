using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NetJoin;

public class MatchList : MonoBehaviour
{
    [SerializeField] private MatchDisplay matchPrefab;
    [SerializeField] private Transform matchListContent;

    private NetJoin netJoin;

    private void Awake()
    {
        netJoin = GetComponent<NetJoin>();
    }

    private void Start()
    {

#if UNITY_WEBGL
        // dont host in webgl build
#else
        HostMatch();
#endif
    }

    public void HostMatch()
    {
        NetManager.singleton.StartHost();
        StartCoroutine(netJoin.HostMatch(OnMatchHosted));
    }

    private void OnMatchHosted(bool success)
    {

    }

    public void Refresh()
    {
        foreach (Transform child in matchListContent)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(netJoin.GetAllMatches(OnGottenMatches));
    }

    private void OnGottenMatches(List<Match> matches)
    {
        foreach (Match match in matches)
        {
            MatchDisplay matchDisplay = Instantiate(matchPrefab, matchListContent);
            matchDisplay.SetMatch(match);
        }
    }
}
