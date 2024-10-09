using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSwitchInterect : ObjectInterect
{
    public VentStateSystem _vent;

    public override void Interect()
    {
        Debug.LogWarning("Switch Vent");

        _vent.TurnOnn();
    }
}
