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
        i++;

        if (_ventObj.Length <= i)
        {
            Debug.LogWarning("END");
            Suck(obj, 0);

            return;
        }

        if(_ventState[i] != VentState.Suck)
        {
            Debug.LogWarning("Blow");

            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _ventObj[i].transform.position;
            obj.GetComponent<NavMeshAgent>().enabled = true;

            return;
        }

        if (i == _ventIn)
        {
            Debug.LogWarning("GAME END");

            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _endVent.transform.position;

            return;
        }

        else
        {
            Debug.LogWarning("USE");
            Suck(obj, i);
        }
    }
}
