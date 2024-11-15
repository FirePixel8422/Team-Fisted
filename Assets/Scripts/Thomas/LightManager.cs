using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using System;


[BurstCompile]
public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    private void Awake()
    {
        Instance = this;
    }




    public float flickerDistance;
    public float tabletFlickerDistance;

    public float disableDistance;
    public float camsDisableDistance;




    public float updateInterval;

    [Header("Monster Close Light Flicker")]
    public float minFlickerTime;
    public float maxFlickerTime;


    [Header("\nSpecial Events")]
    public float eventChanceInterval;


    [Header("Blood Lights Event")]
    public float bloodLightsChance;
    public float bl_MinDuration, bl_MaxDuration;
    public Color lightColor, bloodLightColor;
    public float bl_addedIntensity;
    public float bl_ColorSwapSpeed;

    private bool bloodLightsActive;
    private bool bloodLightsEnding;

    [Header("Destroy Lights Event")]
    public float destroyLightsChance;

    private bool destroyLightsActive;
    private bool destroyLightsEnding;


    [Header("Debug Data")]
    public Light[] lights;
    public Vector3[] lightPos;
    public float[] lightIntensity;
    public bool[] isBloodlight;
    public float[] timeToFlicker;
    public bool[] lightsActive;


    private Transform enemy;
    private Transform player;

    public bool cameraActive;
    public Vector3 activeCameraPos;


    private Unity.Mathematics.Random random;

    private WaitForSeconds updateDelay;




    [BurstCompile]
    private void Start()
    {
        enemy = EnemyAI.Instance.transform;
        player = Movement.Instance.transform;

        random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);

        updateDelay = new WaitForSeconds(updateInterval);


        Light[] _lights = transform.root.GetComponentsInChildren<Light>(true);

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

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }


    [BurstCompile]
    private IEnumerator UpdateLightsTickLoop()
    {
        float elapsed = 0;

        while (true)
        {
            yield return updateDelay;

            UpdatePlayerMonsterBasedLights(Time.time);



            elapsed += updateInterval;

            if (elapsed >= eventChanceInterval)
            {
                elapsed = 0;

                if (bloodLightsActive == false && random.NextFloat(0, 100) <= bloodLightsChance)
                {
                    bloodLightsActive = true;
                    StartCoroutine(BloodLights());
                }

                //if (destroyLightsActive == false && random.NextFloat(0, 100) <= destroyLightsChance)
                //{
                //    destroyLightsActive = true;
                //    StartCoroutine(DestroyLights());
                //}
            }
        }
    }


    public void UpdateActiveCamera(Vector3 camPos, bool reset = false)
    {
        if (reset)
        {
            cameraActive = false;
            UpdatePlayerMonsterBasedLights(0f);
            return;
        }

        activeCameraPos = new Vector3(camPos.x, 0, camPos.z);
        cameraActive = true;
        UpdatePlayerMonsterBasedLights(0f);
    }




    [BurstCompile]
    private IEnumerator BloodLights()
    {
        float startTime = Time.time;

        float totalElapsedTime = 0;
        float elapsedTime;


        float duration = random.NextFloat(bl_MinDuration, bl_MaxDuration);

        while (true)
        {
            yield return updateDelay;

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
    private IEnumerator DestroyLights()
    {
        while (true)
        {
            yield return updateDelay;

        }
    }




    [BurstCompile]
    private void UpdatePlayerMonsterBasedLights(float cTime)
    {
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        Vector3 enemyPos = new Vector3(enemy.position.x, 0, enemy.position.z);

        float disableDistanceSqr = disableDistance * disableDistance;
        float camsDisableDistanceSqr = camsDisableDistance * camsDisableDistance;
        float flickerDistanceSqr = flickerDistance * flickerDistance;
        float tabletFlickerDistanceSqr = tabletFlickerDistance * tabletFlickerDistance;


        if ((enemyPos - playerPos).sqrMagnitude < tabletFlickerDistanceSqr)
        {
            Monitor.Instance.monsterClose = true;
        }
        else
        {
            Monitor.Instance.monsterClose = false;
        }


        for (int i = 0; i < lights.Length; i++)
        {
            bool lightOutOfPlayerRange = (lightPos[i] - playerPos).sqrMagnitude > disableDistanceSqr;
            bool lightOutOfActiveCameraRange = cameraActive == false || (lightPos[i] - activeCameraPos).sqrMagnitude > camsDisableDistanceSqr;

            //if player OR the active camera (if using the tablet) is out of range of target Light
            if (lightOutOfPlayerRange && lightOutOfActiveCameraRange)
            {
                lights[i].gameObject.SetActive(false);
                lightsActive[i] = false;
            }
            else
            {
                bool lightInEnemyRange = (lightPos[i] - enemyPos).sqrMagnitude < flickerDistanceSqr;

                //if light is in range of enemy
                if (lightInEnemyRange)
                {
                    //if flickering is false
                    if (timeToFlicker[i] == 0)
                    {
                        timeToFlicker[i] = cTime + random.NextFloat(minFlickerTime, maxFlickerTime);
                    }
                    else if (cTime >= timeToFlicker[i])
                    {
                        lightsActive[i] = !lightsActive[i];
                        lights[i].gameObject.SetActive(lightsActive[i]);

                        timeToFlicker[i] = cTime + random.NextFloat(minFlickerTime, maxFlickerTime);
                    }
                }
                else if (lightsActive[i] == false)
                {
                    lights[i].gameObject.SetActive(true);
                    lightsActive[i] = true;

                    //set flickering to false
                    timeToFlicker[i] = 0;
                }
            }
        }
    }


    [BurstCompile]
    private void UpdateBloodLights(float elapsedTime)
    {
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);

        float disableDistanceSqr = disableDistance * disableDistance;
        float camsDisableDistanceSqr = camsDisableDistance * camsDisableDistance;


        for (int i = 0; i < lights.Length; i++)
        {
            bool lightInPlayerRange = (lightPos[i] - playerPos).sqrMagnitude <= disableDistanceSqr;
            bool lightInActiveCameraRange = cameraActive == true && (lightPos[i] - activeCameraPos).sqrMagnitude < camsDisableDistanceSqr;

            if (lightInPlayerRange || lightInActiveCameraRange)
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
            else
            {
                if (lights[i] == null)
                {
                    Debug.LogError("LIGHT IS NULL");
                }

                if (isBloodlight[i] == true || bloodLightsEnding == true)
                {
                    lights[i].color = lightColor;

                    lights[i].intensity = lightIntensity[i];

                    isBloodlight[i] = false;
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