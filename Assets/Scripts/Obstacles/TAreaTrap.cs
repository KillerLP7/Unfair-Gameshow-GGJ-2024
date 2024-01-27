using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAreaTrap : BaseObs, ITriggerdObstacle
{
    [SerializeField] private GameObject _areaObj;
    [SerializeField] private float _liveTime;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _triggerd = false; // lul
    

    // Start is called before the first frame update
    void Start()
    {
        _areaObj.SetActive(false);
        if (_liveTime < 0) _liveTime = 2;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void TriggerEffect()
    {
        if (_triggerd) return;
        _triggerd = true;
        StartCoroutine(ActiveAndWait());
    }

    private IEnumerator ActiveAndWait()
    {
        _areaObj.SetActive(true);
        //TODO::Sound
        yield return new WaitForSeconds(_liveTime);
        _areaObj.SetActive(false);
        yield return new WaitForSeconds(_liveTime);

        _triggerd = false;
    }
    
}
