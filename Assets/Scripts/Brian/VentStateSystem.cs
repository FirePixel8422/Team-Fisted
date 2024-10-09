using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentStateSystem : MonoBehaviour
{
    public VentSystem _ventSystem;

    bool _onn = false;

    int _index;

    public void TurnOnn()
    {
        if (_onn == false)
        {
            for (int i = 0; i < _ventSystem._ventObj.Length; i++)
            {
                if (_ventSystem._ventObj[i] == gameObject)
                {
                    _ventSystem._ventState[i] = VentState.Suck;

                    _index = i;
                    _onn = true;
                }
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
