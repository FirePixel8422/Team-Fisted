using UnityEngine;
using UnityEngine.AI;

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

    public void TurVentsOn()
    {
        for (int i = 0; i < _ventState.Length; i++)
        {
            _ventState[i] = VentState.Suck;
        }
    }

    public int _ventIn;

    public void SuckEnemy(GameObject obj, int i)
    {
        _ventIn = i;

        Suck(obj, i);
    }

    public void Suck(GameObject obj, int i)
    {
        Debug.LogWarning("suck Enemy");

        i++;

        if (_ventState[i] != VentState.Suck)
        {
            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _ventObj[i].transform.position;
            obj.GetComponent<NavMeshAgent>().enabled = true;
            Debug.LogWarning("2");
        }

        else if (_ventObj.Length <= i)
        {
            SuckEnemy(obj, 0);
        }

        else if (i == _ventIn)
        {
            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _endVent.transform.position;
        }

        else
        {
            SuckEnemy(obj, i);
        }
    }
}
