using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSwitchInterect : ObjectInterect
{
    public VentStateSystem _vent;
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interect()
    {
        Debug.LogWarning("Switch Vent");

        _animator.SetBool("Switch", true);
        _vent.TurnOnn();
    }
}
