using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectDoor : ObjectInterect
{
    public Animator _animatorL, _animatorR;

    public bool _open = false;

    public override void Interect()
    {
        Debug.LogWarning("Open Door");

        _animatorL.SetBool("Open", true);
        _animatorR.SetBool("Open", true);

        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);

        _animatorL.SetBool("Open", false);
        _animatorR.SetBool("Open", false);
    }
}
