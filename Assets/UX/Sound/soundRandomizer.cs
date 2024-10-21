using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
/*
public enum State
{
    none,
    Normal,
    Chase,
    Sprint,
}*/
public class soundRandomizer : MonoBehaviour
{
    #region Variables
    Input _input;

    InputAction _move;
    InputAction _rotate;
    InputAction _sprint;

    public int arrayRange;

    public AudioSource source;
    public AudioClip[] audioClip; 
    public AudioClip currentAudioClip;

    public int randomNumber;

    public float timer;
    
    public float soundCooldown;
    public float minCooldown, maxCooldown;
    public float minCooldownChase, maxCooldownChase;

    public bool chaseStateOn = false;
    public bool sprintStateOn = false;
    public GameObject enemy;
    //public GameObject player;

    public State state;
    #endregion
    
    
    
    /*
    // Start is called before the first frame update
    void Start()
    {
        state = state.Normal;
        arrayRange = audioClip.Length;

        randomNumber = Random.Range(0, arrayRange);
        soundCooldown = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(Normal());
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                StartCoroutine(Normal());

                break;

            case State.Chase:
                StartCoroutine(Chase());

                break;

            case State.Sprinting:
                StartCoroutine(Sprint());

                break;          
        }

        timer += Time.deltaTime;
    }
    
    
    public IEnumerator Normal()
    {
       yield return new WaitForSeconds(soundCooldown);

        Debug.LogWarning("Sound Play");

        currentAudioClip = audioClip[randomNumber];
        source.clip = currentAudioClip;
        source.Play();

        soundCooldown = Random.Range(minCooldown, maxCooldown);
        randomNumber = Random.Range(0, arrayRange);

        //Check if enemy is in CHASE MODE
        if (enemy.GetComponent<EnemyAI>()._state == State.chase && chaseStateOn == true)
        {
            Debug.LogWarning("ChaseMode");
            state = State.chase;
        }

        //Check is player is SPRINTING
        if (_sprint.enabled == true && sprintStateOn == true)
        {
            state = State.sprint;
        }
    }

    public IEnumerator Chase()
    {
        yield return new WaitForSeconds(soundCooldown);

        Debug.LogWarning("Sound Play");

        currentAudioClip = audioClip[randomNumber];
        source.clip = currentAudioClip;
        source.Play();

        soundCooldown = Random.Range(minCooldownChase, maxCooldownChase);
        randomNumber = Random.Range(0, arrayRange);

        StartCoroutine(Chase());

        //Check if enemy is in CHASE MODE
        if (enemy.GetComponent<EnemyAI>()._state != State.chase)
        {
            Debug.LogWarning("ChaseMode");
            state = State.Normal;
        }
    }
    
    public IEnumerator Sprint()
    {
        yield return new WaitForSeconds(soundCooldown);

        Debug.LogWarning("Sound Play");

        currentAudioClip = audioClip[randomNumber];
        source.clip = currentAudioClip;
        source.Play();

        soundCooldown = Random.Range(minCooldownChase, maxCooldownChase);
        randomNumber = Random.Range(0, arrayRange);

        //Check is player is SPRINTING
        if (_sprint.enabled == true && sprintStateOn == true)
        {
            state = State.sprint;
        }
        StartCoroutine(Sprint());
    }
    */
}
