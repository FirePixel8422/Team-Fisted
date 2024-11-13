using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectDoor : ObjectInterect
{
    public Animator _animatorL, _animatorR;

    public AudioClip audioOpen;
    public AudioClip audioClose;

    public AudioSource _source;

    public bool _open = false;
    public override void Interect()
    {
        if (_open == false)
        {
            _open = true;
            //Debug.LogWarning("Open Door");

            _animatorL.SetBool("Open", true);
            _animatorR.SetBool("Open", true);

            //Play door open sound
            _source.clip = audioOpen;
            _source.Play();
            StartCoroutine(Delay());
        }
        
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);

        //Play door close sound
        _source.clip = audioClose;
        _source.Play();

        yield return new WaitForSeconds(2);

        _animatorL.SetBool("Open", false);
        _animatorR.SetBool("Open", false);

        yield return new WaitForSeconds(2);
        _open = false;
    }


}
