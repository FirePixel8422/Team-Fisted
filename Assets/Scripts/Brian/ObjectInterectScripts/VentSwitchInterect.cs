using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSwitchInterect : ObjectInterect
{
    public VentStateSystem _vent;
    Animator _animator;

    public override void StartV()
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
