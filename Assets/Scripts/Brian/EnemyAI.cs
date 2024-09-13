using System;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    none,
    Wandering,
    Sound,
    WanderSound,
    chase
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
        public NavMeshAgent _agent;
        public Vector3 _audioLoc;
        public Transform _player;
    }

    Vector3 _lastSoundLoc;

    public void Start()
    {
        _components._agent.speed = _speed;

        _state = State.Wandering;
    }


    private void Update()
    {
        switch (_state)
        {
            case State.Wandering:
                RandomRaoming();

                break;

            case State.Sound:
                GoToAudio();

                break;

            case State.chase:

                FindPlayer();
                break;
        }
    }

    public void RandomRaoming()
    {
       if(_components._agent.remainingDistance < 1)
       {
            _components._agent.SetDestination(RandomNavmeshLocation(10));
       }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void HearSound(Vector3 context)
    {
        _components._audioLoc = context;

        _state = State.Sound;
    }

    public void GoToAudio()
    {
        _components._agent.SetDestination(_components._audioLoc);
    }

    public void FindPlayer()
    {

    }

    public void Vent()
    {

    }
}
