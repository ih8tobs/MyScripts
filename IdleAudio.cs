using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private int clipIndex;
    public AudioSource audioSource;
    

    void Start()
    {
        
    }
    void Update()
    {

        if (!audioSource.isPlaying)
        {

            clipIndex = Random.Range(0, clips.Length);
            audioSource.clip = clips[clipIndex];
            audioSource.PlayDelayed(Random.Range(10f, 20f));
            Debug.Log("Nothing playing, we set new audio to " + audioSource.clip.name);
        }
    }
}
