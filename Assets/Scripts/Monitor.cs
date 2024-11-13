using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    public Camera monitorCamera;

    public CameraController[] gameCameras;

    public Transform staticScreenCamHolder;
    public float camSwapDelayMin, camSwapDelayMax;

    public int selectedCameraIndex;

    public bool isSwappingCamera;



    private void Start()
    {
        monitorCamera = GetComponentInChildren<Camera>();

        gameCameras = FindObjectsOfType<CameraController>();

        gameCameras[selectedCameraIndex].SetupCamera(monitorCamera);
    }


    private void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            if (isSwappingCamera == false)
            {
                isSwappingCamera = true;
                StartCoroutine(ChangeCamera());
            }
        }
    }

    private IEnumerator ChangeCamera()
    {
        selectedCameraIndex += 1;

        if (selectedCameraIndex == gameCameras.Length)
        {
            selectedCameraIndex = 0;
        }

        monitorCamera.transform.SetParent(staticScreenCamHolder, false, false);

        yield return new WaitForSeconds(Random.Range(camSwapDelayMin, camSwapDelayMax));

        gameCameras[selectedCameraIndex].SetupCamera(monitorCamera);

        isSwappingCamera = false;
    }
}
