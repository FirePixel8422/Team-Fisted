using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VentStateSystem : MonoBehaviour
{
    public VentSystem _ventSystem;
    Animator _animator;
    public GameObject smokePar;
    public RawImage rawImage;
    public EnemyAI _enemy;

    bool _onn = false;

    int _index;

    //SFX
    public AudioSource _audioSource;
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

            //SFX
            _audioSource.Play();
            rawImage.color = Color.green;

            //go to vent sound when turning on
            _enemy.HearSound(transform.position);
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
        //Debug.LogWarning(other.name + " enter vent trigger");

        if (_ventSystem._ventState[_index] == VentState.Suck)
        {
            _ventSystem.SuckEnemy(other.gameObject, _index);
        }
    }
}
