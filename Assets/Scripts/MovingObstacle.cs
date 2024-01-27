using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : BaseObs
{
    private Vector2 _startPosition;
    [SerializeField] private Vector2 _endPosition;

    [SerializeField] private AnimationCurve curve;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _endPosition += _startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position = Vector2.Lerp(_startPosition, _endPosition, curve.Evaluate(time));
    }

    private void OnDrawGizmos()
    {
        // _startPosition = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_startPosition, _endPosition);
    }

}
