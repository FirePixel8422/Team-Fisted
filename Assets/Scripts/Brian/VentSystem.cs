using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VentState
{
    Off,
    Blow,
    Suck
}

public class VentSystem : MonoBehaviour
{
    public GameObject[] _ventObj;
    public VentState[] _ventState;

    public GameObject _endVent;

    void SetState(int i, VentState state)
    {
        _ventState[i] = state;
    }

    public void SuckEnemy(GameObject obj, int i)
    {
        i = i++;

        if (_ventState.Length >= i)
        {
            obj.transform.position = _endVent.transform.position;
        }

        if (_ventState[i] != VentState.Blow)
        {
            obj.transform.position = _ventObj[i].transform.position;
        }

        else
        {
            SuckEnemy(obj, i);
        }

    }
}
