using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<EventReference> eventRefStrings;
    EventInstance eventInstance;
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
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventRefStrings[0]);
        eventInstance.start();
        eventInstance.release();
    }
}
