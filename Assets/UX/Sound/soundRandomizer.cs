using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Stated
{
    none,
    Normal,
    Chase,
    Walk,
    Sprint,
}
public class soundRandomizer : MonoBehaviour
{
    #region Variables
    Input _input;

    InputAction _move;
    InputAction _sprint;

    public Vector2 _moveDirection;

    public int arrayRange;

    public AudioSource source;
    public AudioClip[] audioClip; 
    public AudioClip currentAudioClip;

    public int randomNumber;

    public float timer;
    
    public float soundCooldown;
    public float minCooldown, maxCooldown;
    public float minCooldownChase, maxCooldownChase;
    public float minCooldownSprint, maxCooldownSprint;

    public bool chaseStateOn = false;
    public bool sprintStateOn = false;
    public bool sprintOn = false;
    public GameObject enemy;

    public bool walk = false;
    //public GameObject player;

    Stated stated;
    #endregion

    private void Awake()
    {
        _input = new Input();
    }

    private void OnEnable()
    {
        _input.Enable();

        _move = _input.Movement.Move;
        _sprint = _input.Movement.Sprint;

        _sprint.started += SprintToggle;
        _sprint.canceled += SprintToggle;
    }

    public void SprintToggle(InputAction.CallbackContext context)
    {
        if (context.started && sprintStateOn)
        {
            sprintOn = true;
        }

        else
        {
            sprintOn = false;
        }
    }

    public void Move(Vector2 context)
    {
        _moveDirection = context;
    }

    // Start is called before the first frame update
    void Start()
    {
        stated = Stated.Normal;
        arrayRange = audioClip.Length;

        randomNumber = Random.Range(0, arrayRange);
        soundCooldown = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(Normal());
    }
    
    // Update is called once per frame
    void Update()
    {
        Move(_move.ReadValue<Vector2>());

        switch (stated)
        {
            case Stated.Normal:
                StartCoroutine(Normal());

                break;

            case Stated.Chase:
                StartCoroutine(Chase());

                break;

            case Stated.Walk:
                StartCoroutine(Walk());

                break;

            case Stated.Sprint:
                StartCoroutine(Sprint());

                break;          
        }

        timer += Time.deltaTime;

        //Check if WALK script
        if (sprintStateOn)
        {
            //Check if playing is SPRINTING
            if (sprintOn)
            {
                stated = Stated.Sprint;
            }
            else
            {
                stated = Stated.Walk;
            }
        }

        //Check if enemy is in CHASE MODE
        if (enemy.GetComponent<EnemyAI>()._state == State.chase && chaseStateOn)
        {
            //Debug.LogWarning("ChaseMode");
            stated = Stated.Chase;
        }       
    }  
    
    public IEnumerator Normal()
    {
        //Debug.LogWarning("Normal Mode");
        yield return new WaitForSeconds(soundCooldown);

        if (soundCooldown <= timer)
        {
            //Debug.LogWarning("Sound Play");
            timer = 0;
            currentAudioClip = audioClip[randomNumber];
            source.clip = currentAudioClip;
            source.Play();

            soundCooldown = Random.Range(minCooldown, maxCooldown);
            randomNumber = Random.Range(0, arrayRange);
        }    
    }

    public IEnumerator Chase()
    {
        //Debug.LogWarning("Chase Mode");
        yield return new WaitForSeconds(soundCooldown);

        if (soundCooldown <= timer)
        {
            //Debug.LogWarning("Sound Play");
            timer = 0;
            currentAudioClip = audioClip[randomNumber];
            source.clip = currentAudioClip;
            source.Play();

            soundCooldown = Random.Range(minCooldownChase, maxCooldownChase);
            randomNumber = Random.Range(0, arrayRange);

            StartCoroutine(Chase());
        }
    }
    public IEnumerator Walk()
    {
        //Debug.LogWarning("Walk Mode");
        yield return new WaitForSeconds(soundCooldown);

        if (_moveDirection != Vector2.zero)
        {
            if (soundCooldown <= timer)
            {
                //Debug.LogWarning("Sound Play");
                timer = 0;
                currentAudioClip = audioClip[randomNumber];
                source.clip = currentAudioClip;
                source.Play();

                soundCooldown = Random.Range(minCooldown, maxCooldown);
                randomNumber = Random.Range(0, arrayRange);
            }
        }
    }
    public IEnumerator Sprint()
    {
        //Debug.LogWarning("Sprint Mode");
        yield return new WaitForSeconds(soundCooldown);

        if (_moveDirection != Vector2.zero)
        {
            if (soundCooldown <= timer)
            {
                //Debug.LogWarning("Sound Play");
                timer = 0;
                currentAudioClip = audioClip[randomNumber];
                source.clip = currentAudioClip;
                source.Play();

                soundCooldown = Random.Range(minCooldownSprint, maxCooldownSprint);
                randomNumber = Random.Range(0, arrayRange);

                StartCoroutine(Sprint());
            }
        }
            
    }
}
