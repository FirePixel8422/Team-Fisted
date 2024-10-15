using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterect : ObjectInterect
{
    public GameObject _camerUI;

    public Movement _player;

    public CameraControler _controler;

    public override void Interect()
    {
        if (!_camerUI.activeSelf)
        {
            _controler.CameraSwitch();
            _camerUI.SetActive(true);

            _player.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            _camerUI.SetActive(false);

            EnableMouse();
        }
    }

    public void EnableMouse()
    {
        _controler.CameraSwitch();
        _player.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
