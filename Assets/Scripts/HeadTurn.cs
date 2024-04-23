using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTurn : NetworkBehaviour
{
    
    public GameObject cam;
    public GameObject playerHead;
    public GameObject bodyTurn;
    Quaternion rotation;


    private void Start()
    {
        cam = Camera.main.gameObject;
    }
    private void Update()
    {
        /*playerHead.rotation = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

        playerHead.forward = cam.transform.forward;
        Quaternion rotation = playerHead.rotation;
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
        playerHead.rotation = rotation;*/

        //playerHead.transform.forward = cam.transform.forward;
        if (HasStateAuthority) { 
            playerHead.transform.rotation = Quaternion.Euler(playerHead.transform.eulerAngles.x, cam.transform.eulerAngles.y, playerHead.transform.eulerAngles.x);
            bodyTurn.transform.forward = cam.transform.forward;

        }
    }
}
