using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public float minCooldown, maxCooldown;

    //public bool chaseMode;
    //public int chaseMinCooldown;
    //public int chaseMaxCooldown;


    // Start is called before the first frame update
    void Start()
    {
        arrayRange = audioClip.Length;

        randomNumber = Random.Range(0, arrayRange);
        soundCooldown = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(DelaySounds());
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //if (chaseMode == true)
        //{
        //    minCooldown += chaseMinCooldown;
        //    maxCooldown += chaseMaxCooldown;
        //}
        //else
        //{
        //    minCooldown -= chaseMinCooldown;
        //    maxCooldown -= chaseMaxCooldown;
        //}
        if (timer > soundCooldown)
        {
            timer = 0;
                        
            
            
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
        
        
    }
    public IEnumerator DelaySounds()
    {
        yield return new WaitForSeconds(soundCooldown);

        Debug.LogWarning("Eerie Sound");

        currentAudioClip = audioClip[randomNumber];
        source.clip = currentAudioClip;
        source.Play();

        soundCooldown = Random.Range(minCooldown, maxCooldown);
        randomNumber = Random.Range(0, arrayRange);
        StartCoroutine(DelaySounds());
    }
}
