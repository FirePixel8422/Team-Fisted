using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectDoor : ObjectInterect
{
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interect()
    {
        Debug.LogWarning("Open Door");

        _animator.SetBool("Switch", true);
    }
}
