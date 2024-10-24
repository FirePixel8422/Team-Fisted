using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : ObjectInterect
{
    public AudioSource _source;

    public EnemyAI _enemy;

    bool _onn = false;

    public override void Interect()
    {
        if (_onn)
        {
            _source.Stop();
            _enemy.HearSound(transform.position);
            StartCoroutine(TurnOff());

            _onn = false;

            return;
        }

        else
        {
            _source.Play();
            StopCoroutine(TurnOff());

            _onn = true;

            return;
        }

        IEnumerator TurnOff()
        {
            yield return new WaitForSeconds(33);

            Interect();
        }
    }
}
