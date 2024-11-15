using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


[BurstCompile]
public class Monitor : MonoBehaviour
{
    public static Monitor Instance;
    private void Awake()
    {
        Instance = this;
    }




    public Camera monitorCamera;
    public AudioSource staticSound;
    private float staticSoundClipLength;

    public Animator playerTabletFlashLigthAnim;
    public float grabTime;

    public CameraController[] gameCameras;



    [Header("Load and Logo Show Camera logic")]

    public Transform staticScreenCamHolder;
    public float camSwapDelayMin, camSwapDelayMax;

    public Transform monitorLogoCamHolder;
    public Animator monitorLogoAnim;
    public float monitorLogoShowTime;

    public Transform mapCamHolder;
    public Animator mapAnim;

    public bool resetCameraIndexAtRestart;


    private int selectedCameraIndex = -1;

    public bool isSwappingCamera;
    public static bool monitorActive;

    public GameObject TEMPGAMEOBJECTMONITOR;

    private Coroutine openMonitorCO;
    private Unity.Mathematics.Random random;



    private void Start()
    {
        gameCameras = FindObjectsOfType<CameraController>();

        staticSoundClipLength = staticSound.clip.length;

        random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
    }


    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (monitorActive == false)
            {
                monitorActive = true;
                openMonitorCO = StartCoroutine(EnableMonitor());
            }
            else
            {
                monitorActive = false;

                if (openMonitorCO != null)
                {
                    StopCoroutine(openMonitorCO);
                }
                DisableMonitor();
            }
        }


        if (monitorActive && isSwappingCamera == false)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                isSwappingCamera = true;
                StartCoroutine(ChangeCamera(-1));
            }
            else if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                isSwappingCamera = true;
                StartCoroutine(ChangeCamera(1));
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Tab))
            {
                isSwappingCamera = true;
                StartCoroutine(ChangeCameraToMap());
            }
        }
    }

    private IEnumerator EnableMonitor()
    {
        playerTabletFlashLigthAnim.SetBool("EquipTablet", true);

        yield return new WaitForSeconds(grabTime);

        isSwappingCamera = true;

        monitorCamera.enabled = true;
        monitorCamera.transform.SetParent(monitorLogoCamHolder, false, false);

        monitorLogoAnim.SetTrigger("Play");

        yield return new WaitForSeconds(monitorLogoShowTime);

        StartCoroutine(ChangeCamera(0));
    }

    public void DisableMonitor()
    {
        playerTabletFlashLigthAnim.SetBool("EquipTablet", false);

        monitorCamera.transform.SetParent(staticScreenCamHolder, false, false);
        monitorCamera.enabled = false;

        if (resetCameraIndexAtRestart)
        {
            selectedCameraIndex = -1;
        }

        LightManager.Instance.UpdateActiveCamera(Vector3.zero, true);
    }




    private IEnumerator ChangeCamera(int change)
    {
        selectedCameraIndex += change;

        if (selectedCameraIndex == gameCameras.Length)
        {
            selectedCameraIndex = 0;
        }
        else if (selectedCameraIndex == -1)
        {
            selectedCameraIndex = gameCameras.Length - 1;
        }


        LightManager.Instance.UpdateActiveCamera(gameCameras[selectedCameraIndex].transform.position);

        yield return StartCoroutine(PlayStaticScreen());

        gameCameras[selectedCameraIndex].SetupCamera(monitorCamera);

        isSwappingCamera = false;
    }


    private IEnumerator ChangeCameraToMap()
    {
        LightManager.Instance.UpdateActiveCamera(Vector3.zero, true);

        PlayerMap.Instance.UpdateMap();

        yield return StartCoroutine(PlayStaticScreen());

        //mapAnim.SetTrigger("Play");
        monitorCamera.transform.SetParent(mapCamHolder, false, false);

        isSwappingCamera = false;
    }


    private IEnumerator PlayStaticScreen()
    {
        staticSound.Play();
        staticSound.time = random.NextFloat(0, staticSoundClipLength);

        monitorCamera.transform.SetParent(staticScreenCamHolder, false, false);

        yield return new WaitForSeconds(random.NextFloat(camSwapDelayMin, camSwapDelayMax));

        staticSound.Stop();
    }
}
