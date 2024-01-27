using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NewControls cotrols;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    private InputAction _moveAction;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    private float _distToGround;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _distToGround = _boxCollider.bounds.extents.y;
    }
    void Awake()
    {
        _moveAction = GetComponent<PlayerInput>().actions.FindAction("move");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -5.5f)
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = _moveAction.ReadValue<Vector2>();
        moveVector.x *= speed;
        //TODO::Sound
        _rb.velocity = new Vector2(moveVector.x, _rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        //if OnJump gets called per "performed" apply jump force
        //TODO::Sound
        if (!ctx.performed) return;
        print("Grounded:" + IsGrounded().ToString());
        if (!IsGrounded()) return;
        _rb.AddForce(new Vector2(0, _jumpForce));
        print("Jump");
    }

    private bool IsGrounded()
    {
        //Checks, with a box under the player obj, if the player is on the ground
        Vector2 pointVector = new Vector2(transform.position.x, transform.position.y - _distToGround - 0.1f);
        Vector2 sizeVector = new Vector2(_boxCollider.size.x, _boxCollider.size.y);
        Collider2D hitCollider = Physics2D.OverlapBox(pointVector, sizeVector, 0.0f, _groundCheckLayerMask);

        if (hitCollider == null) return false;
        else return true;
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawCube(new Vector3(transform.position.x, (transform.position.y - _distToGround - 0.1f), transform.position.z), new Vector3((_boxCollider.size.x),_boxCollider.size.y, 1.0f));
    }
}
