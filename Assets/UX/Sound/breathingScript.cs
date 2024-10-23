using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breathingScript : MonoBehaviour
{
    public GameObject enemy;
    public AudioSource breathSFX;
    public AudioSource heartbeat;
    public float lerpSpeed;
    private float temp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetComponent<EnemyAI>()._state == State.chase)
        {
            if (heartbeat.pitch < 1.5f)
            {
                heartbeat.pitch = Mathf.Lerp(0, 1.5f, temp);
                breathSFX.volume = Mathf.Lerp(0, 1, temp);
                temp += lerpSpeed * Time.deltaTime;
                if (heartbeat.pitch == 1.5f)
                {
                    temp = 0;
                    heartbeat.pitch = 1.5f;
                    breathSFX.volume = 1;
                }
            }
        }
        else
        {
            if (heartbeat.pitch > 0)
            {
                heartbeat.pitch = Mathf.Lerp(1.5f, 0, temp);
                breathSFX.volume = Mathf.Lerp(1, 0 , temp);
                temp += lerpSpeed * Time.deltaTime;
                if (heartbeat.pitch == 0)
                {
                    temp = 0;
                    heartbeat.pitch = 0f;
                    breathSFX .volume = 0;
                }
            }
        }
    }
}
