using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour
{
    public float dieTime;

    public void Start()
    {
        dieTime = Time.time + dieTime;
    }
    public void Update()
    {
        if (Time.time > dieTime)
        {
            Destroy(gameObject);
        }
    }
}
