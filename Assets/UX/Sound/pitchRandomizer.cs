using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pitchRandomizer : MonoBehaviour
{
    public int arrayRange;
    public AudioClip[] audioClip; 
    public AudioClip currentAudioClip;
    public int randomNumber;
    public AudioSource source;
    public int listInt = 1;
    public float timer;
    public float soundCooldown;
    public int minCooldown;
    public int maxCooldown;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

        
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > soundCooldown)
        {
            timer = 0;
                        
            
            randomNumber = Random.Range(0, arrayRange);
            currentAudioClip = audioClip[randomNumber];
            PlayTrack(currentAudioClip);
        }
    }
    //private IEnumerator SoundSelect()
    //{
        
    //    PlayTrack(randomNumber);
        
    //}
    public void PlayTrack(AudioClip audioclipIndex)
    {

        //currentAudioClip = audioClip[randomNumber];
        source.clip = currentAudioClip;
        source.Play();
        soundCooldown = Random.Range(minCooldown, maxCooldown);
    }
}
