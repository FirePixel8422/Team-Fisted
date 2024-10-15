using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Camera[] _cameras;

    public bool _cameraOnn;

    public void CameraSwitch()
    {
        if (_cameraOnn)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].enabled = false;
            }

            _cameraOnn = false;
        }

        else if (!_cameraOnn)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].enabled = true;
            }

            _cameraOnn = true;
        }
    }
}
