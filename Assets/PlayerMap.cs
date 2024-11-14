using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMap : MonoBehaviour
{
    public static PlayerMap Instance;
    private void Awake()
    {
        Instance = this;
    }


    public Transform mapParent;

    public Transform playerIcon;
    public Vector3 mapScale;
    public Vector3 playerIconCenterPos;

    public Vector3 mapCenter;
    public Vector3 mapDimensions;



    private void Update()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        Vector3 playerPos = Movement.Instance.transform.position;

        playerIcon.transform.position = playerIconCenterPos + new Vector3((playerPos.x + mapCenter.x) / mapDimensions.x * mapScale.x, 0, (playerPos.z + mapCenter.z) / mapDimensions.z * mapScale.z);
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(mapParent.transform.position + mapCenter, mapDimensions);
    }
}
