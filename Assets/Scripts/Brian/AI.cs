using System;
using System.Collections.Generic;
using UnityEngine;

public enum AiState
{
    Idle,
    Warning,
    OnGard,
    BackOff,
    Attack,
    InCombat
}

public enum Ai
{
    Defensive,
    aggressive
}

public class AI : MonoBehaviour
{
    [Range(0, 100)]
    public float _skill;

    [Range(0, 20)]
    public float _lookDistance;

    List<Vector3> _lastKnowEnemyLocationList = new List<Vector3>();
    Vector3 _lastKnowEnemyLocation;
    Vector3 _lateLastKnowEnemyLocation;


    public Components _components = new Components();
    public Brain _brain = new Brain();

    [Serializable]
    public class Components
    {
        public Camera _camera;

        public GameObject[] _target;
        public Transform _emptyTarget;

        public Transform _defencePoint;
    }

    float _timer;

    private void Awake()
    {
        IdleRandomLocation();
    }

    [Serializable]
    public class Brain
    {
        public AiState _state;
        public Ai _ai;
    }

    private void FixedUpdate()
    {
        BrainPower();
        Vision();
    }

    private void LateUpdate()
    {
        _lateLastKnowEnemyLocation = _lastKnowEnemyLocation;
    }

    void Vision()
    {
        foreach (GameObject target in _components._target)
        {
            if (Vector3.Distance(_components._camera.transform.position, target.transform.position) <= _lookDistance)
            {


                Vector3 viewPos = _components._camera.WorldToViewportPoint(target.transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    _lastKnowEnemyLocationList.Add(target.transform.position);

                    _lastKnowEnemyLocation = _lastKnowEnemyLocationList[_lastKnowEnemyLocationList.Count - 1];

                    if (_brain._ai == Ai.Defensive)
                    {
                        if (Vector3.Distance(transform.position, _components._defencePoint.position) <= 5)
                        {
                            _brain._state = AiState.InCombat;
                        }

                        else if (_brain._state != AiState.InCombat)
                        {
                            _brain._state = AiState.BackOff;
                        }
                    }

                    else
                    {
                        _brain._state = AiState.InCombat;
                    }
                }

                else
                {
                    if (_brain._ai == Ai.Defensive && _brain._state != AiState.InCombat)
                    {
                        _brain._state = AiState.BackOff;
                    }

                    else
                    {
                        _brain._state = AiState.Attack;
                    }
                }
            }
        }
    }

    Vector3 RandomLocation;
    void IdleRandomLocation()
    {
        if (_brain._ai == Ai.aggressive)
        {
            RandomLocation = UnityEngine.Random.onUnitSphere * 15;
            RandomLocation += transform.position;
            RandomLocation.y = 0;
        }

        else if (_brain._ai == Ai.Defensive)
        {
            RandomLocation = UnityEngine.Random.onUnitSphere * 15;
            RandomLocation += _components._defencePoint.position;
            RandomLocation.y = 0;
        }
    }

    void OnGardRandomLocation()
    {
        RandomLocation = UnityEngine.Random.onUnitSphere * 5;
        RandomLocation += _lastKnowEnemyLocation;
        RandomLocation.y = 0;
    }


    void WalkTo()
    {
        Debug.Log("lll");

        //Quaternion targetRotation = Quaternion.LookRotation(RandomLocation - transform.position, Vector3.up);
        //transform.localRotation = Quaternion.Lerp(transform.rotation, targetRotation, _skill / 25 * Time.deltaTime);

        //Vector3 velocity = Vector3.zero;
        //transform.localPosition = Vector3.SmoothDamp(transform.position, RandomLocation, ref velocity, 0.3f, _skill / 2);

        //_components._emptyTarget.position = RandomLocation;
    }

    void BackOff()
    {
        _components._emptyTarget.position = _lastKnowEnemyLocation;
        float _dictance = Vector3.Distance(transform.position, _components._emptyTarget.position);

        Quaternion targetRotation = Quaternion.LookRotation(_lastKnowEnemyLocation - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _skill / 25 * Time.deltaTime);

        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, transform.position - (_lastKnowEnemyLocation - transform.position).normalized * _dictance, ref velocity, 0.3f, (_skill / 2));
    }

    void GoToLastEnemyLocation(Vector3 location)
    {
        Quaternion targetRotation = Quaternion.LookRotation(location - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _skill / 25 * Time.deltaTime);

        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, location, ref velocity, 0.3f, _skill / 2);

        _components._emptyTarget.position = location;

        Debug.Log("llll");

        if (Vector3.Distance(transform.position, _components._emptyTarget.position) <= 10)
        {
            _brain._state = AiState.OnGard;

            Debug.Log("lllll");
        }
    }

    public void BrainPower()
    {
        switch (_brain._state)
        {
            case AiState.Idle:
                {
                    WalkTo();

                    if (Vector3.Distance(transform.position, _components._emptyTarget.position) <= 10)
                    {
                        IdleRandomLocation();
                    }

                    return;
                }

            case AiState.Warning:
                {
                    return;
                }

            case AiState.OnGard:
                {
                    WalkTo();
                    _timer += Time.deltaTime;

                    if (Vector3.Distance(transform.position, _components._emptyTarget.position) <= 2)
                    {
                        OnGardRandomLocation();
                    }

                    if (_timer > 20)
                    {
                        _timer = 0;
                        _brain._state = AiState.Idle;
                    }

                    return;
                }

            case AiState.BackOff:
                {
                    BackOff();

                    if (Vector3.Distance(transform.position, _components._emptyTarget.position) >= 5)
                    {
                        IdleRandomLocation();
                        _brain._state = AiState.Idle;
                    }

                    return;
                }

            case AiState.Attack:
                {
                    if (Vector3.Distance(transform.position, _lastKnowEnemyLocation) >= 1)
                    {
                        GoToLastEnemyLocation(_lastKnowEnemyLocation);
                    }

                    return;
                }

            case AiState.InCombat:
                {
                    if (Vector3.Distance(transform.position, _lastKnowEnemyLocation) >= 1)
                    {
                        GoToLastEnemyLocation(_lastKnowEnemyLocation);
                    }

                    return;
                }
        }
    }
}
