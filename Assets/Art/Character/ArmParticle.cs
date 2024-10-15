using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ArmParticle : MonoBehaviour
{
    public GameObject[] spawnplaces;
    public GameObject armRight, armLeft;
    public float spawnAfterSeconds;
    private float timeStamp;
    private bool switchArm;
    private int random;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeStamp)
        {
            GetRandomInt();
            if (switchArm)
            {
                Instantiate(armRight, spawnplaces[random].transform);
            }
            else
            {
                Instantiate(armLeft, spawnplaces[random].transform);
            }
            switchArm = !switchArm;
            timeStamp = Time.time + spawnAfterSeconds;
        }
    }

    public void GetRandomInt()
    {
        random = Random.Range(0, spawnplaces.Length - 1);
    }
}
