using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSwitchInterect : ObjectInterect
{
    public VentStateSystem _vent;
    Animator _animator;
    // red light turns green;
    public Light light;
    public GameObject lamp;
    public Material greenmat;

    //SFX
    public AudioSource _audioSource;

    public override void StartV()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interect()
    {
        //Debug.LogWarning("Switch Vent");

        _animator.SetBool("Switch", true);
        _vent.TurnOnn();

        light.color = Color.green;
        lamp.GetComponent<Renderer>().material = greenmat;

        //SFX
        _audioSource.Play();
    }
}
