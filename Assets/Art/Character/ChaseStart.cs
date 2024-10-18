using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStart : MonoBehaviour
{
    public float scale, growSpeed;
    public GameObject arms;
    public void OnEnable()
    {
        scale = 0;
        arms.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1, 1, 1), scale);
        if (scale < 1)
        {
            scale += growSpeed * Time.deltaTime;
        }
        else
        {
            arms.SetActive(true);
        }
        //gameObject.GetComponent<ParticleSystem>().startColor.
    }
}
