using System.Collections;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    public Vector2 _delayTime;

    Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(_delayTime.x, _delayTime.y));

        if (!_light.isActiveAndEnabled) StartCoroutine(Delay());

        else
        {
            _light.enabled = false;
            yield return new WaitForSeconds(.2f);
            _light.enabled = true;
            yield return new WaitForSeconds(.2f);
            _light.enabled = false;
            yield return new WaitForSeconds(Random.Range(_delayTime.x / 20, _delayTime.y / 20));
            _light.enabled = true;
            StartCoroutine(Delay());
        }
    }
}
