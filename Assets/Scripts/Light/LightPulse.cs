using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    private Light light;
    public float minStrength, maxStrength, pulseSpeed;

    private float lerpTime = 0.0f; // A timer to control the interpolation
    private bool increasing = true; // Direction of the pulse

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the lerp time based on pulse speed
        lerpTime += (increasing ? 1 : -1) * pulseSpeed * Time.deltaTime;

        // Reverse direction if we reach the limits
        if (lerpTime >= 1.0f)
        {
            lerpTime = 1.0f;
            increasing = false; // Change direction
        }
        else if (lerpTime <= 0.0f)
        {
            lerpTime = 0.0f;
            increasing = true; // Change direction
        }

        // Lerp the intensity based on lerpTime
        light.intensity = Mathf.Lerp(minStrength, maxStrength, lerpTime);
    }
}

