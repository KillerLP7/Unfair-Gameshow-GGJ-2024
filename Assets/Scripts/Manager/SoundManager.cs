using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] EventReference deathRefStrings;
    EventInstance deathInstance;
    [SerializeField] EventReference duckRefStrings;
    EventInstance duckInstance;
    [SerializeField] EventReference flingRefStrings;
    EventInstance flingInstance;
    [SerializeField] EventReference jumpRefStrings;
    EventInstance jumpInstance;
    [SerializeField] EventReference fireRefStrings;
    EventInstance fireInstance;
    [SerializeField] EventReference stepRefStrings;
    EventInstance stepInstance;
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayDeathSound()
    {
        print("DeathSound");
        deathInstance = FMODUnity.RuntimeManager.CreateInstance(deathRefStrings);
        deathInstance.start();
        deathInstance.release();
    }
    public void PlayDuckSound()
    {
        print("DuckSound");
        duckInstance = FMODUnity.RuntimeManager.CreateInstance(duckRefStrings);
        duckInstance.start();
        duckInstance.release();
    }
    public void PlayFlingSound()
    {
        print("FlingSound");
        flingInstance = FMODUnity.RuntimeManager.CreateInstance(flingRefStrings);
        flingInstance.start();
        flingInstance.release();
    }
    public void PlayJumpSound()
    {
        print("FlingSound");
        jumpInstance = FMODUnity.RuntimeManager.CreateInstance(jumpRefStrings);
        jumpInstance.start();
        jumpInstance.release();
    }
    public void PlayStepSound()
    {
        print("DeathSound");
        stepInstance = FMODUnity.RuntimeManager.CreateInstance(stepRefStrings);
        stepInstance.start();
        stepInstance.release();
    }
}
