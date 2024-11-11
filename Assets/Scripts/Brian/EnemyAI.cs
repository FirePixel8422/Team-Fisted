using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Runtime.CompilerServices;

public enum State
{
    none,
    Wandering,
    Sound,
    WanderSound,
    chase,
    WanderPlayer,
    goToVent,
    Venting,
    GameEnd,
}

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;
    


    public State _state;

    [Header("")]
    public float _speed;

    [Header("")]
    public Components _components = new Components();

    [Serializable]
    public class Components
    {
        public GameObject _normalEnemy;
        public GameObject _chaseEnemy;

        [HideInInspector] public Vector3 _lastAudioLoc;

        [HideInInspector] public List<Vector3> _lastKnowEnemyLocationList = new List<Vector3>();
        [HideInInspector] public Vector3 _lastEnemyLoc;

        [HideInInspector] public Vector3 _goVentLoc;

        public NavMeshAgent _agent;
        public Camera _camera;

        public GameObject[] _target;
        public GameObject[] _ventLoc;
        public GameObject[] _spawnLoc;

        public Vector2 _ventDelayTime;
        public float _ventTime;

        [Header("")]

        public GameObject _gameLost;

        // Ro Jumpscare on dead
        public GameObject jumpscare;
    }

    private void Awake()
    {
        Instance = this;

        _components._agent.enabled = false;
        transform.position = _components._spawnLoc[UnityEngine.Random.Range(0, _components._spawnLoc.Length)].transform.position;
        _components._agent.enabled = true;
    }

    public void Start()
    {
        _components._agent.speed = _speed;
        _components._ventTime = UnityEngine.Random.Range(_components._ventDelayTime.x, _components._ventDelayTime.y);

        _state = State.Wandering;

        _components._chaseEnemy.SetActive(false);
    }

    private void OnEnable()
    {
        _state = State.Wandering;

        _components._agent.SetDestination(RandomNavmeshLocationLoc(transform.position, 50));
    }


    private void Update()
    {
        if (_state == State.GameEnd) return;

        Vision();

        switch (_state)
        {
            case State.Wandering:
                RandomRaoming();

                break;

            case State.Sound:
                GoToSound();

                break;

            case State.chase:
                GoToPlayer();

                break;

            case State.WanderSound:
                WanderAudioLoc();

                break;

            case State.WanderPlayer:
                WanderPlayerLoc();

                break;

            case State.goToVent:
                Vent();

                break;
        }
        
    }

    public void RandomRaoming()
    {
        _components._ventTime -= Time.deltaTime;

        if(_components._ventTime <= 0)
        {
            _components._goVentLoc = Vector3.zero;
            _state = State.goToVent;
        }

        if (_components._agent.remainingDistance < 1)
        {
            _components._agent.SetDestination(RandomNavmeshLocationLoc(_components._target[0].transform.position, 100));
        }
    }

    public void HearSound(Vector3 context)
    {
        _components._lastAudioLoc = context;

        _state = State.Sound;
    }

    public void GoToSound()
    {
        _components._agent.SetDestination(RandomNavmeshLocationLoc(_components._lastAudioLoc, 10));

        if (_components._agent.remainingDistance <= 0)
        {
            _state = State.WanderSound;
        }
    }
    public void WanderAudioLoc()
    {
        if (_components._agent.remainingDistance < 1)
        {
            _components._agent.SetDestination(RandomNavmeshLocationLoc(_components._lastAudioLoc, 10));
        }

        StartCoroutine(SwitchStateDelay(15, State.Wandering));
    }

    void Vision()
    {
        foreach (GameObject target in _components._target)
        {
            if (Vector3.Distance(_components._camera.transform.position, target.transform.position) <= 20)
            {
                Vector3 viewPos = _components._camera.WorldToViewportPoint(target.transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    _components._lastKnowEnemyLocationList.Add(target.transform.position);

                    _components._lastEnemyLoc = _components._lastKnowEnemyLocationList[_components._lastKnowEnemyLocationList.Count - 1];

                    _state = State.chase;

                    _components._chaseEnemy.SetActive(true);
                    // Ro Start Coroutine
                    StartCoroutine(DeactivateIdle(2f));

                    StopCoroutine(SwitchStateDelay(15, State.WanderPlayer));
                }
            }
        }
    }

    public void GoToPlayer()
    {
        _components._agent.SetDestination(_components._lastEnemyLoc);

        if(_components._agent.remainingDistance <= 5)
        {
            if (Vector3.Distance(_components._target[0].transform.position, transform.position) <= 5)
            {
                KillPlayer();
            }

            else
            {
                _state = State.WanderPlayer;
            }
        }
    }

    public void WanderPlayerLoc()
    {
        StartCoroutine(SwitchStateDelay(15, State.Wandering));
        // Ro Switch de active enemy terug als enemy speler kwijt is.
        _components._normalEnemy.SetActive(true);
        _components._chaseEnemy.SetActive(false);

        if (_components._agent.remainingDistance < 1)
        {
            _components._agent.SetDestination(RandomNavmeshLocationLoc(_components._lastEnemyLoc, 10));
        }
    }

    float _ventTimeStuck;

    public void Vent()
    {
        if (_components._goVentLoc == Vector3.zero)
        {
            _ventTimeStuck = 0;

            _components._ventTime = UnityEngine.Random.Range(_components._ventDelayTime.x, _components._ventDelayTime.y);

            _components._goVentLoc = _components._ventLoc[UnityEngine.Random.Range(0, _components._ventLoc.Length)].transform.position;

            _components._agent.SetDestination(_components._goVentLoc);
        }

        _ventTimeStuck += Time.deltaTime;

        if (_components._agent.remainingDistance <= 0.1 || _ventTimeStuck > 30)
        {
            _state = State.Venting;
            Venting();
        }
    }

    public void Venting()
    {
        _components._agent.enabled = false;
        _components._goVentLoc = _components._ventLoc[UnityEngine.Random.Range(0, _components._ventLoc.Length)].transform.position;

        transform.position = _components._goVentLoc;
        _components._agent.enabled = true;

        _components._agent.SetDestination(RandomNavmeshLocationLoc(transform.position, 20));

        _state = State.Wandering;

    }

    public Vector3 RandomNavmeshLocationLoc(Vector3 location,float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += location;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool _swicthOn;

    IEnumerator SwitchStateDelay(float context, State state)
    {
        if (!_swicthOn)
        {
            _swicthOn = true;
            yield return new WaitForSeconds(context);
            _state = state;
            _swicthOn = false;
        }
    }

    public void KillPlayer()
    {
        _state = State.GameEnd;

        _components._gameLost.SetActive(true);

        _components._target[0].GetComponent<Movement>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        

        Jumpscare();

        Debug.LogError("dead");
    }
    // Ro Nieuwe Coroutine om de normale enmey uit te zetten na 2 secondenn zodat de chase versie eerst volledig is gegroeit.
    private IEnumerator DeactivateIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        _components._normalEnemy.SetActive(false);
        StopCoroutine(DeactivateIdle(0));
    }
   

    private void Jumpscare()
    {
        Instantiate(_components.jumpscare, new Vector3(100,0,0), Quaternion.identity);
    }

}
