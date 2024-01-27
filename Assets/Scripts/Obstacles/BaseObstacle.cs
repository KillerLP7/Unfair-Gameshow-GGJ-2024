using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObs : MonoBehaviour
{
    private Collider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController pPayerController)) return;
        GameManager.Instance.PlayerReset();
    }
}
