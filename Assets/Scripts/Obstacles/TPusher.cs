using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPusher : BaseObs, ITriggerdObstacle
{
    [SerializeField] private float _strength;
    private Rigidbody2D _rb;
    private float _defaultGravity;

    // Start is called before the first frame update
    void Start()
    {
       _rb = GetComponent<Rigidbody2D>();
        _defaultGravity = _rb.gravityScale;
        _rb.gravityScale = 0.0f;
    }

    public void TriggerEffect()
    {
        _rb.gravityScale = _defaultGravity;
        _rb.AddForce((Vector2.up * _strength));
        SoundManager.Instance.PlayFlingSound();
    }
}
