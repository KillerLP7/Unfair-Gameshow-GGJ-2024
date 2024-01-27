using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeTrap : MonoBehaviour
{
    private BoxCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController pPayerController)) return;
        StartCoroutine(UIManager.Instance.SchowCake());
        Destroy(gameObject);
    }
}
