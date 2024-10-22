using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breathingScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject breathSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetComponent<EnemyAI>()._state == State.chase)
        {
            breathSFX.SetActive(true);
        }
        else
        {
            breathSFX.SetActive(false);
        }
    }
}
