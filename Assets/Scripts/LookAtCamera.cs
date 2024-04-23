using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    DeattachCamera camLoc;

    private void Start()
    {
        camLoc = GetComponent<DeattachCamera>();
    }
    private void Update()
    {
        transform.LookAt(camLoc.transform.position);
    }
}
