using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject toFollow;
    [SerializeField] private float _lerpValue;
    [SerializeField] private float _lerpRadius;

    [SerializeField] private Vector2 bounds = new(float.MinValue, float.MaxValue);

    //TODO:: has to be called somewhere
    public void SetBounds(Vector2 newBounds)
    {
        bounds = newBounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (toFollow == null) return;

        float posX = toFollow.transform.position.x;
        posX = Mathf.Clamp(posX, bounds.x, bounds.y);
        posX = Mathf.Lerp(transform.position.x, posX, _lerpValue * Time.deltaTime * 2);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}

