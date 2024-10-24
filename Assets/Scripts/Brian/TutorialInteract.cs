using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteract : ObjectInterect
{
    public Movement _player;

    public GameObject _ui;

    public override void Interect()
    {
        if (!_ui.activeSelf)
        {
            _player.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _ui.SetActive(true);
        }
    }
}
