using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentStateSystem : MonoBehaviour
{
    public VentSystem _ventSystem;
    Animator _animator;
    public GameObject smokePar;

    bool _onn = false;

    int _index;
    private void Start()
    {
        _animator = GetComponent<Animator>();

        for (int i = 0; i < _ventSystem._ventObj.Length; i++)
        {
            if (_ventSystem._ventObj[i] == gameObject)
            {
                _index = i;
            }
        }
    }

    public void TurnOnn()
    {
        if (_onn == false)
        {
            _ventSystem._ventState[_index] = VentState.Suck;

            _animator.SetBool("Switch", true);
            smokePar.SetActive(false);
            _onn = true;
        }
    }

    public void Switch()
    {
        if (_onn)
        {
            if (_ventSystem._ventState[_index] == VentState.Blow)
            {
                _ventSystem._ventState[_index] = VentState.Suck;
            }

            else if (_ventSystem._ventState[_index] == VentState.Suck)
            {
                _ventSystem._ventState[_index] = VentState.Blow;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.name + " enter vent trigger");

        if (_ventSystem._ventState[_index] == VentState.Suck)
        {
            _ventSystem.SuckEnemy(other.gameObject, _index);
        }
    }
}
