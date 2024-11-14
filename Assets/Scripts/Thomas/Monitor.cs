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

    public CameraController[] gameCameras;



    [Header("Load and Logo Show Camera logic")]

    public Transform staticScreenCamHolder;
    public float camSwapDelayMin, camSwapDelayMax;

    public Transform monitorLogoCamHolder;
    public Animator monitorLogoAnim;
    public float monitorLogoShowTime;

    public bool resetCameraIndexAtRestart;


    private int selectedCameraIndex = -1;

    public bool isSwappingCamera;
    public static bool monitorActive;

    public GameObject TEMPGAMEOBJECTMONITOR;



    private void Start()
    {
        gameCameras = FindObjectsOfType<CameraController>();
    }


    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (monitorActive == false)
            {
                monitorActive = true;
                StartCoroutine(EnableMonitor());
            }
            else
            {
                monitorActive = false;
                DisableMonitor();
            }
        }


        if (monitorActive && (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.P)))
        {
            if (isSwappingCamera == false)
            {
                isSwappingCamera = true;
                StartCoroutine(ChangeCamera());
            }
        }
    }

    private IEnumerator EnableMonitor()
    {
        isSwappingCamera = true;
        TEMPGAMEOBJECTMONITOR.SetActive(true);

        Movement.Instance.enabled = false;

        monitorCamera.enabled = true;
        monitorCamera.transform.SetParent(monitorLogoCamHolder, false, false);

        monitorLogoAnim.SetTrigger("Play");

        yield return new WaitForSeconds(monitorLogoShowTime);

        StartCoroutine(ChangeCamera());
    }

    public void DisableMonitor()
    {
        TEMPGAMEOBJECTMONITOR.SetActive(false);

        Movement.Instance.enabled = true;

        monitorCamera.transform.SetParent(staticScreenCamHolder, false, false);
        monitorCamera.enabled = false;

        if (resetCameraIndexAtRestart)
        {
            selectedCameraIndex = -1;
        }

        LightManager.Instance.UpdateActiveCamera(Vector3.zero, true);
    }


    private IEnumerator ChangeCamera()
    {
        selectedCameraIndex += 1;

        if (selectedCameraIndex == gameCameras.Length)
        {
            selectedCameraIndex = 0;
        }

        LightManager.Instance.UpdateActiveCamera(gameCameras[selectedCameraIndex].transform.position);


        monitorCamera.transform.SetParent(staticScreenCamHolder, false, false);

        yield return new WaitForSeconds(Random.Range(camSwapDelayMin, camSwapDelayMax));

        gameCameras[selectedCameraIndex].SetupCamera(monitorCamera);

        isSwappingCamera = false;
    }
}
