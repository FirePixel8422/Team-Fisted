using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using System;
using System.Threading;


[BurstCompile]
public class LightManager : MonoBehaviour
{
    public float flickerDistance;
    public float disableDistance;

    public float updateInterval;

    [Header("Monster Close Light Flicker")]
    public int minFlickerTime, maxFlickerTime;

    [Header("Blood Lights Event")]
    public float bloodLightsChance;
    public float bl_UpdateInterval;
    public float bl_MinDuration, bl_MaxDuration;
    public Color lightColor, bloodLightColor;
    public float bl_addedIntensity;
    public float bl_ColorSwapSpeed;

    private bool bloodLightsActive;
    private bool bloodLightsEnding;


    [Header("Debug Data")]
    public Light[] lights;
    public Vector3[] lightPos;
    public float[] lightIntensity;
    public bool[] isBloodlight;
    public float[] timeToFlicker;
    public bool[] lightsActive;


    private Transform enemy;
    private Transform player;

    private Unity.Mathematics.Random random;




    [BurstCompile]
    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        enemy = EnemyAI.Instance.transform;
        player = Movement.Instance.transform;

        random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);


        Light[] _lights = FindObjectsOfType<Light>(true);

        lights = new Light[_lights.Length - 1];
        lightPos = new Vector3[_lights.Length - 1];
        lightIntensity = new float[_lights.Length - 1];
        isBloodlight = new bool[_lights.Length - 1];
        timeToFlicker = new float[_lights.Length - 1];
        lightsActive = new bool[_lights.Length - 1];


        int addedLightIndex = 0;
        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i].CompareTag("Player"))
            {
                continue;
            }

            lights[addedLightIndex] = _lights[i];
            lights[addedLightIndex].gameObject.SetActive(false);

            lightPos[addedLightIndex] = new Vector3(_lights[i].transform.position.x, 0, _lights[i].transform.position.z);
            lightIntensity[addedLightIndex] = _lights[i].intensity;

            addedLightIndex += 1;
        }

        StartCoroutine(UpdateLightsTickLoop());
    }


    [BurstCompile]
    private IEnumerator UpdateLightsTickLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        float time;

        while (true)
        {
            yield return wait;

            time = Time.time;

            UpdateLights(time);

            if (bloodLightsActive == false && random.NextFloat(0, 100) <= bloodLightsChance)
            {
                bloodLightsActive = true;
                StartCoroutine(BloodLights());
            }
        }
    }



    public float totalElapsedTime = 0;

    [BurstCompile]
    private IEnumerator BloodLights()
    {
        WaitForSeconds wait = new WaitForSeconds(bl_UpdateInterval);

        float time = Time.time;
        float startTime = time;
        float elapsedTime;


        float duration = random.NextFloat(bl_MinDuration, bl_MaxDuration);

        while (true)
        {
            yield return wait;

            //save previous totalElapsedTime
            elapsedTime = totalElapsedTime;

            totalElapsedTime = Time.time - startTime;

            //new totalElpasedTime - totalElapsedTime = elapsedTime
            elapsedTime = totalElapsedTime - elapsedTime;

            UpdateBloodLights(elapsedTime);

            if (totalElapsedTime > duration)
            {
                bloodLightsEnding = true;
            }
            if (totalElapsedTime > duration + 5)
            {
                bloodLightsActive = false;
                bloodLightsEnding = false;
                yield break;
            }
        }
    }




    [BurstCompile]
    private void UpdateLights(float cTime)
    {
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        Vector3 enemyPos = new Vector3(enemy.position.x, 0, enemy.position.z);

        for (int i = 0; i < lights.Length; i++)
        {
            if ((lightPos[i] - playerPos).sqrMagnitude > disableDistance * disableDistance)
            {
                if (lightsActive[i] == true)
                {
                    lights[i].gameObject.SetActive(false);
                    lightsActive[i] = false;
                }
            }

            else if ((lightPos[i] - enemyPos).sqrMagnitude < flickerDistance * flickerDistance)
            {
                //if flickering is false
                if (timeToFlicker[i] == 0)
                {
                    timeToFlicker[i] = cTime + random.NextInt(minFlickerTime, maxFlickerTime) * 0.1f;
                }
                else if (cTime >= timeToFlicker[i])
                {

                    lightsActive[i] = !lightsActive[i];
                    lights[i].gameObject.SetActive(lightsActive[i]);

                    timeToFlicker[i] = cTime + random.NextInt(minFlickerTime, maxFlickerTime) * 0.1f;
                }
            }

            else
            {
                if (lightsActive[i] == false)
                {
                    lights[i].gameObject.SetActive(true);
                    lightsActive[i] = true;
                }

                //set flickering to false
                timeToFlicker[i] = 0;
            }
        }
    }


    [BurstCompile]
    private void UpdateBloodLights(float elapsedTime)
    {
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);

        for (int i = 0; i < lights.Length; i++)
        {
            if ((lightPos[i] - playerPos).sqrMagnitude < disableDistance * disableDistance)
            {
                if (isBloodlight[i] == true || bloodLightsEnding == true)
                {
                    Color targetColor = ColorMoveTowards(lights[i].color, lightColor, bl_ColorSwapSpeed * elapsedTime);
                    lights[i].color = targetColor;

                    float targetIntensity = IntensityMoveTowards(lights[i].intensity, lightIntensity[i], bl_ColorSwapSpeed * bl_addedIntensity * elapsedTime);
                    lights[i].intensity = targetIntensity;

                    if (targetColor == lightColor && targetIntensity == lightIntensity[i])
                    {
                        isBloodlight[i] = false;
                    }
                }
                else if (bloodLightsEnding == false)
                {
                    Color targetColor = ColorMoveTowards(lights[i].color, bloodLightColor, bl_ColorSwapSpeed * elapsedTime);
                    lights[i].color = targetColor;

                    float targetIntensity = IntensityMoveTowards(lights[i].intensity, lightIntensity[i] + bl_addedIntensity, bl_ColorSwapSpeed * bl_addedIntensity * elapsedTime);
                    lights[i].intensity = targetIntensity;

                    if (targetColor == bloodLightColor && targetIntensity == (lightIntensity[i] + bl_addedIntensity))
                    {
                        isBloodlight[i] = true;
                    }
                }
            }
        }


        [BurstCompile]
        static float IntensityMoveTowards(float value, float target, float maxStep)
        {
            float difference = target - value;

            // If the difference is smaller than maxStep, move directly to target
            if (math.abs(difference) <= maxStep)
            {
                return math.clamp(target, 0, target); // Clamp target intensity within an acceptable range
            }

            // Move towards the target by maxStep and clamp the result
            return math.clamp(value + math.sign(difference) * maxStep, 0, target);
        }


        static Color ColorMoveTowards(Color current, Color target, float maxDelta)
        {
            return new Color(
                MoveTowardsChannel(current.r, target.r, maxDelta),
                MoveTowardsChannel(current.g, target.g, maxDelta),
                MoveTowardsChannel(current.b, target.b, maxDelta),
                MoveTowardsChannel(current.a, target.a, maxDelta)
            );

            static float MoveTowardsChannel(float current, float target, float maxDelta)
            {
                float difference = target - current;

                // If the difference is smaller than maxDelta, move directly to target
                if (Mathf.Abs(difference) <= maxDelta)
                {
                    return Mathf.Clamp01(target); // Ensure the target is clamped
                }

                // Move towards the target by maxDelta and clamp the result
                return Mathf.Clamp01(current + Mathf.Sign(difference) * maxDelta);
            }
        }
    }
}