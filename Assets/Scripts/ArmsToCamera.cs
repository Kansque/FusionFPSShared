using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ArmsToCamera : NetworkBehaviour
{
    public CinemachineVirtualCamera cam;
    NetworkPlayer player;

    private void Start()
    {
        
    }

    private void Update()
    {
        //if (HasStateAuthority) { 
            transform.forward = Camera.main.transform.forward;
            Debug.Log(gameObject.name.ToString());
      
        //}
    }
}
