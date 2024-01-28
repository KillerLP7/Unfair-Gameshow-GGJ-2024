using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODOnNotWebGL : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_WEBGL
#elif UNITY_SERVER
#else
        StudioListener listener = gameObject.AddComponent<StudioListener>();
#endif
    }
}
