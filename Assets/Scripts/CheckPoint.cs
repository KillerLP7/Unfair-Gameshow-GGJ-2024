using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPoint : MonoBehaviour
{
    private GameObject _playerObj;
    private BoxCollider2D _boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController pPayerController)) return;
        GameManager.Instance.UpdateCheckPoint(gameObject);
    }
}
