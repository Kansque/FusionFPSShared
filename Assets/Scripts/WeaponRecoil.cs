using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Vector3 currentPosition, targetPosition, initialGunPosition;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float kickBack;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        Camera.main.transform.localRotation = Quaternion.Euler(currentRotation);
        KickBack();
    }

    public void RecoilFire()
    {
        targetPosition -= new Vector3(0, 0, kickBack);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    void KickBack()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnSpeed);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}
