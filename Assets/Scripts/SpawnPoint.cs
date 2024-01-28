using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
