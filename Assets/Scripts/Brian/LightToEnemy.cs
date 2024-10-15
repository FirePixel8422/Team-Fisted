using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToEnemy : MonoBehaviour
{
    public float _lightDistence;

    Light _light;
    public GameObject _enemy;

    private void Start()
    {
        _light = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, _enemy.transform.position) < _lightDistence)
        {
            _light.enabled = false;
        }

        else if(_light.enabled == false)
        {
            _light.enabled = true;
        }
    }
}
