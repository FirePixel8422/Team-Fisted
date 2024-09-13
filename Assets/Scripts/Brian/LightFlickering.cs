using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    public Vector2 _delayTime;

    Light light;

    private void Start()
    {
        light = GetComponent<Light>();

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(_delayTime.x, _delayTime.y));

        light.enabled = false;
        yield return new WaitForSeconds(Random.Range(_delayTime.x / 20, _delayTime.y / 20));
        light.enabled = true;
        StartCoroutine(Delay());
    }
}
