using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject _victory;

    public Movement _player;

    private void OnTriggerEnter(Collider other)
    {
        _victory.SetActive(true);

        _player.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        
    }
}
