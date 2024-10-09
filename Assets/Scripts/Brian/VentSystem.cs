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
        Debug.LogWarning("suck Enemy");

        i += 1;

        if (_ventState[i] != VentState.Suck)
        {
            obj.transform.position = _ventObj[i].transform.position;
            Debug.LogWarning("2");
        }

        else
        {
            SuckEnemy(obj, i);
            Debug.LogWarning("3");
        }

    }
}
