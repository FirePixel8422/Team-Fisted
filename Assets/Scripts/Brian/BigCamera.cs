using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCamera : MonoBehaviour
{
    public GameObject[] _bigCamera;

    public GameObject _big;

    private void OnEnable()
    {
        for (int i = 0; i < _bigCamera.Length; i++)
        {
            _bigCamera[i].SetActive(false);
        }
    }

    public void BigCameraV(int i)
    {
        for (int y = 0; y < _bigCamera.Length; y++)
        {
            _bigCamera[y].SetActive(false);
        }

        _big.SetActive(true);
        _bigCamera[i].SetActive(true);
    }
}
