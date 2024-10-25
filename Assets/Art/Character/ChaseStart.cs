using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStart : MonoBehaviour
{
    public float scale, growSpeed;
    public GameObject arms;
    private ParticleSystem.MainModule mainModule;
    private Color startColor;
    public void OnEnable()
    {
        scale = 0;
        arms.SetActive(false);
        mainModule = gameObject.GetComponent<ParticleSystem>().main;
        startColor = mainModule.startColor.color;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1, 1, 1), scale);
        startColor.a = Mathf.Lerp(0, 1, scale);
        mainModule.startColor = startColor;
        if (scale < 1)
        {
            scale += growSpeed * Time.deltaTime;
        }
        else
        {
            arms.SetActive(true);
        }
       
    }
}
