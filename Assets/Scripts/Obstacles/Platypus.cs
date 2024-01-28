using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private bool _bFacesRight;
    private CapsuleCollider2D _capsule;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private LayerMask _playerCheckLayerMask;
    private float _capsuleWidth;
    private Vector3 sizeVector = new Vector3(1.2f, 1.2f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        _bFacesRight = true;
        _direction = new Vector2(1.0f, 0.0f);
        _rb = GetComponent<Rigidbody2D>();
        _capsule = GetComponent<CapsuleCollider2D>();
        _capsuleWidth = _capsule.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _direction * _moveSpeed;
        CheckForTurn();
        CheckForPlayer();
    }

    void CheckForTurn()
    {

        Vector3 pointVector = new Vector3(transform.position.x + _capsuleWidth, transform.position.y, transform.position.z);
        Collider2D hitCollider = Physics2D.OverlapBox(pointVector, sizeVector, 0.0f, _groundCheckLayerMask);
        if (hitCollider == null)
        {
            if (_bFacesRight)
            {
                _capsuleWidth = -_capsuleWidth;
                _direction.x = -_direction.x;
                _bFacesRight = false;

            }
            else
            {
                _capsuleWidth = -_capsuleWidth;
                _direction.x = -_direction.x;
                _bFacesRight = true;
            }
        }

    }
    void CheckForPlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(transform.position, sizeVector, 0.0f, _playerCheckLayerMask);
        if (hitCollider != null)
        {
            SoundManager.Instance.PlayDuckSound();
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController pPayerController)) return;
        SoundManager.Instance.PlayDuckSound();
        GameManager.Instance.PlayerReset();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), sizeVector);
    }
}
