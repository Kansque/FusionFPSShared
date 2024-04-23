using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LocalCamera : MonoBehaviour
{
    float mouseX, mouseY;
    float camRotationX, camRotationY;
    Camera cam;
    public Transform bodyAnchor;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam.enabled)
            cam.transform.parent = null;
    }

    private void Update()
    {
        //if(!cam.enabled)
          //  return;
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        cam.transform.position = bodyAnchor.position;

        camRotationX -= mouseY;
        camRotationX = Mathf.Clamp(camRotationX, -90f, 90f);

        camRotationY += mouseX;

        cam.transform.localRotation = Quaternion.Euler(camRotationX, camRotationY, 0);



    }
}
