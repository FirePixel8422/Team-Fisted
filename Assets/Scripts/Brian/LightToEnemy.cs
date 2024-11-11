using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Obsolete]
public class LightToEnemy : MonoBehaviour
{
    public float _lightDistence;

    Light _light;

    public bool _lightFlicking;
    public GameObject _enemy;

    private void Start()
    {
        _light = GetComponent<Light>();

        StartCoroutine(Timer());
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _enemy.transform.position) < _lightDistence)
        {
            _lightFlicking = true;
        }

        else if (_lightFlicking == true)
        {
            _lightFlicking = false;
        }
    }

    IEnumerator Timer()
    {
        if (_lightFlicking)
        {
            yield return new WaitForSeconds(Random.Range(.1f, 1f));
            _light.enabled = false;
            yield return new WaitForSeconds(Random.Range(.1f, 1f));
            _light.enabled = true;

            StartCoroutine(Timer());
        }

        else
        {
            yield return new WaitForSeconds(2);

            StartCoroutine(Timer());
        }
    }
}
