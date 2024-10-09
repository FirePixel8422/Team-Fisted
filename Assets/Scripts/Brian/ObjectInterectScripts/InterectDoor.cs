using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectDoor : ObjectInterect
{
    public override void Interect()
    {
        Debug.LogWarning("Open Door");
    }
}
