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

    public void SuckEnemy(GameObject obj, int i)
    {
        Debug.LogWarning("suck Enemy");

        i++;

        if (_ventObj.Length <= i)
        {
            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _endVent.transform.position;
        }

        else if (_ventState[i] != VentState.Suck)
        {
            obj.GetComponent<NavMeshAgent>().enabled = false;
            obj.transform.position = _ventObj[i].transform.position;
            obj.GetComponent<NavMeshAgent>().enabled = true;
            Debug.LogWarning("2");
        }

        else
        {
            SuckEnemy(obj, i);
            Debug.LogWarning("3");
        }

    }
}
