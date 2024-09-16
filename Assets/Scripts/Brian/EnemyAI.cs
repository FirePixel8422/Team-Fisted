using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    none,
    Wandering,
    Sound,
    WanderSound,
    chase,
    WanderPlayer
}

public class EnemyAI : MonoBehaviour
{
    public State _state;

    [Header("")]
    public float _speed;

    [Header("")]
    public Components _components = new Components();

    [Serializable]
    public class Components
    {
        [HideInInspector] public Vector3 _lastAudioLoc;

        [HideInInspector] public List<Vector3> _lastKnowEnemyLocationList = new List<Vector3>();
        [HideInInspector] public Vector3 _lastEnemyLoc;

        public NavMeshAgent _agent;
        public Camera _camera;

        public GameObject[] _target;
    }

    public void Start()
    {
        _components._agent.speed = _speed;

        _state = State.Wandering;
    }


    private void Update()
    {
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
        }
    }

    public void RandomRaoming()
    {
        if (_components._agent.remainingDistance < 1)
        {
            _components._agent.SetDestination(RandomNavmeshLocationLoc(transform.position, 20));
        }
    }

    public void HearSound(Vector3 context)
    {
        _components._lastAudioLoc = context;

        _state = State.Sound;
    }

    public void GoToSound()
    {
        _components._agent.SetDestination(_components._lastEnemyLoc);

        if (_components._agent.remainingDistance <= 1)
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

        SwitchStateDelay(15, State.Wandering);
    }

    void Vision()
    {
        foreach (GameObject target in _components._target)
        {
            if (Vector3.Distance(_components._camera.transform.position, target.transform.position) <= 50)
            {
                Vector3 viewPos = _components._camera.WorldToViewportPoint(target.transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    _components._lastKnowEnemyLocationList.Add(target.transform.position);

                    _components._lastEnemyLoc = _components._lastKnowEnemyLocationList[_components._lastKnowEnemyLocationList.Count - 1];

                    _state = State.chase;
                }
            }
        }
    }

    public void GoToPlayer()
    {
        _components._agent.SetDestination(_components._lastEnemyLoc);

        if(_components._agent.remainingDistance <= 1)
        {
            _state = State.WanderPlayer;
        }
    }

    public void WanderPlayerLoc()
    {
        if (_components._agent.remainingDistance < 1)
        {
            _components._agent.SetDestination(RandomNavmeshLocationLoc(_components._lastEnemyLoc, 10));
        }

        SwitchStateDelay(15, State.Wandering);
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

    IEnumerator SwitchStateDelay(float context, State state)
    {
        yield return new WaitForSeconds(context);

        _state = state;
    }
    

    public void Vent()
    {

    }
}
