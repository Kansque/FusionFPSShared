using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;

public class DeattachCamera : NetworkBehaviour
{
    [SerializeField]public CinemachineVirtualCamera cam;
    NetworkObject networkObject;
    [SerializeField] Transform playerCameraRoot;
    public static bool hasAuth = false;

    private void Start()
    {
        if (networkObject.HasStateAuthority) { 
            var obj = Instantiate(cam);
            obj.transform.parent = transform;
            hasAuth = true;
        }
    }
    public override void Spawned()
    {
        //cam = GetComponentInChildren<CinemachineVirtualCamera>();
        networkObject = GetComponent<NetworkObject>();

        if (cam != null && networkObject.HasStateAuthority)
        {
            //cam.transform.parent = null;
            cam.Follow = playerCameraRoot;
        }
    }
}
