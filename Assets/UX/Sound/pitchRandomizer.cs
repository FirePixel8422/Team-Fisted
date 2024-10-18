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
            currentAudioClip = StartCoroutine(SoundSelect());
            PlayTrack();
            soundCooldown = Random.Range(10, 30);
        }
    }
    private IEnumerator SoundSelect()
    {
        randomNumber = Random.Range(0, arrayRange);
        PlayTrack(randomNumber);
        yield return currentAudioClip;
    }
    public void PlayTrack(AudioClip audioclipIndex)
    {
       
        currentAudioClip = audioClip[randomNumber];
        PlayTrack(randomNumber);
        source.Play();
    }
}
