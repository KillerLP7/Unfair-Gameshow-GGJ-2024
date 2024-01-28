using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CakePlaneMovement : MonoBehaviour
{
    private Rigidbody2D _cakeRB;
    private float _defaultGravity;
    // Start is called before the first frame update
    void Start()
    {
        _cakeRB = GetComponent<Rigidbody2D>();
        _defaultGravity = _cakeRB.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerController pC = GameManager.Instance.GetPlayerController();
        if (pC != null)
        {
            if (!pC.GetBIsMoving())
            {
                _cakeRB.gravityScale = 0.0f;
            }
            else _cakeRB.gravityScale = _defaultGravity;
        }
    }
}
