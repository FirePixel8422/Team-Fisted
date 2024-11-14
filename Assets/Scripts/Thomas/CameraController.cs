using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


[BurstCompile]
public class CameraController : MonoBehaviour
{
    public float rotateSpeed;

    public Transform rotatePoint;
    public Transform cameraHolder;

    public float rotStart;
    public float rotAngle;

    public int cameraFrameRate;
    private float cameraframeTime;


    private void Start()
    {
        cameraframeTime = 1 / (float)cameraFrameRate;

        StartCoroutine(RotateCameraLoop());
    }

    private IEnumerator RotateCameraLoop()
    {
        while (true)
        {
            rotatePoint.rotation = Quaternion.Euler(0, rotStart + rotAngle * 0.5f * Mathf.Sin(Time.time * rotateSpeed), 0);

            yield return new WaitForSeconds(cameraframeTime);
        }
    }


    public void SetupCamera(Camera monitorCamera)
    {
        monitorCamera.transform.SetParent(cameraHolder, false, false);
    }






    public float cameraLineLength;
    public Vector3 camerLineStartOffset;


    [BurstCompile]
    private void OnDrawGizmos()
    {
        if(rotatePoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Vector3 cameraForward = rotatePoint.forward * camerLineStartOffset.z;
        Vector3 cameraFOV = rotatePoint.right * camerLineStartOffset.x;

        Vector3 directionAngleLeft = Quaternion.AngleAxis(rotAngle * 0.5f, rotatePoint.up) * rotatePoint.forward;
        Vector3 directionAngleRight = Quaternion.AngleAxis(-rotAngle * 0.5f, rotatePoint.up) * rotatePoint.forward;

        Vector3 endPositionLeft = rotatePoint.position + cameraForward - cameraFOV + directionAngleLeft * cameraLineLength;
        Vector3 endPositionRight = rotatePoint.position + cameraForward + cameraFOV + directionAngleRight * cameraLineLength;


        Gizmos.DrawLine(rotatePoint.position + cameraForward - cameraFOV, endPositionLeft);
        Gizmos.DrawLine(rotatePoint.position + cameraForward + cameraFOV, endPositionRight);
    }
}
