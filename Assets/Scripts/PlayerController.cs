using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NewControls cotrols;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private float _speed;
    private InputAction _moveAction;
    private CircleCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private float _distToGround;
    private float speedMod;
    private bool _bIsMoving;
    private float _leftBound;
    // private InputActionAsset _inputActionA;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<CircleCollider2D>();
        // _inputActionA = GetComponent<InputActionAsset>();
        _distToGround = _boxCollider.bounds.extents.y;
        _bIsMoving = false;
        speedMod = 1.0f;
        SetBound(9);
        SoundManager.Instance.PlayMainTheme();
    }
    void Awake()
    {
        _moveAction = GetComponent<PlayerInput>().actions.FindAction("move");
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb.velocity.x == 0.0f) _bIsMoving = false;
        else _bIsMoving = true;
        if (transform.position.y <= -5.5f)
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    private void FixedUpdate()
    {
        //Check if player can move left, and if the Input Action "left" is getting pressed
        Vector2 moveVector = _moveAction.ReadValue<Vector2>();
        moveVector.x *= (_speed * speedMod);
        //when speed enough SoundManager.Instance.PlayStepSound();
        /*
        if (!(transform.position.x + moveVector.x <= _leftBound))
        {
        }
         */
        _rb.velocity = new Vector2(moveVector.x, _rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        //if OnJump gets called per "performed" apply jump force
        if (!ctx.performed) return;
        if (!IsGrounded()) return;
        SoundManager.Instance.PlayJumpSound();
        _rb.AddForce(new Vector2(0, _jumpForce));
    }

    private bool IsGrounded()
    {
        //Checks, with a box under the player obj, if the player is on the ground
        Vector2 pointVector = new Vector2(transform.position.x, transform.position.y - _distToGround - 0.1f);
        Vector2 sizeVector = new Vector2(_boxCollider.radius * 2, _boxCollider.radius * 2);
        Collider2D hitCollider = Physics2D.OverlapBox(pointVector, sizeVector, 0.0f, _groundCheckLayerMask);

        if (hitCollider == null) return false;
        else return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NextStageTrigger"))
        {
            GameManager.Instance.CheckIsLevelPartByPlayer(collision.transform.parent.gameObject);
            //Limmets the movment of the player to the left
            _leftBound = gameObject.transform.position.x;
            //TODO:: Level Finsied wit index from all player levels 
            GameManager.Instance.LoadNextStage();
            collision.enabled = false;
        }
    }
    public void SetSpeedMod(float pMod) { speedMod = pMod; }
    public void SetBound(float pRadius) { _leftBound = gameObject.transform.position.x - pRadius; }
    public bool GetBIsMoving() { return _bIsMoving; }
    private void OnDrawGizmos()
    {
        // Gizmos.DrawCube(new Vector3(transform.position.x, (transform.position.y - _distToGround - 0.1f), transform.position.z), new Vector3((_boxCollider.size.x),_boxCollider.size.y, 1.0f));
    }

    public bool CheckCanMoveLeft()
    {
        if (gameObject.transform.position.x <= _leftBound) return false;
        else return true;
    }
}
