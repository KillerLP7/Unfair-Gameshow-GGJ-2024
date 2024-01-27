using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject toFollow;
    [SerializeField] private float _lerpValue;
    [SerializeField] private float _lerpRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toFollow != null)
        {
               float lerpedX = Mathf.Lerp(transform.position.x, toFollow.transform.position.x, _lerpValue * Time.deltaTime *2);
               transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
        }
    }
}
