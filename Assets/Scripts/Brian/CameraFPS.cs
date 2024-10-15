using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPS : MonoBehaviour
{
    public float _FPS;

    Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();

        StartCoroutine(FPSTimer());
    }

    IEnumerator FPSTimer()
    {
        _cam.enabled = false;
        yield return new WaitForSeconds(60 / _FPS / 2);
        _cam.enabled = true;
        yield return new WaitForSeconds(60 / _FPS / 2);
        StartCoroutine(FPSTimer());
    }
}
