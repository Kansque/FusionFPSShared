using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    float lastTimeFired = 0f;

    public Transform aimPoint;
    public LayerMask collisionLayers;

    public ParticleSystem muzzleFlash;

    public WeaponRecoil recoil;

    PlayerHealth playerHP;
    NetworkPlayer networkPlayer;
    PlayerData playerData;
    GameObject cam;

    private void Awake()
    {
        playerHP = GetComponent<PlayerHealth>();
        networkPlayer = GetComponent<NetworkPlayer>();
        playerData = GetComponent<PlayerData>();
        //cam = GetComponent<HeadTurn>();
        //cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        cam = Camera.main.gameObject;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;
        //if (playerHP.isDead)
        //   return;

        //if (GetInput(out NetworkInputData data))
        //{
        //if (data.isGunFired)
        //  {
        if (Input.GetMouseButtonDown(0)) { 
            Fire(cam.transform.forward);
            //   }
            //}
            recoil.RecoilFire();
        }

    }

        private void Update()
    {
        if (!HasStateAuthority)
            return;
     
        if (Input.GetMouseButtonDown(0)) { 
            Fire(cam.transform.forward);
            muzzleFlash.Play();
            recoil.RecoilFire();
        }
    }

    void Fire(Vector3 forwardAim)
    {
        if (Time.time - lastTimeFired < 0.15f)
        {
            return;
        }

        Runner.LagCompensation.Raycast(aimPoint.position, forwardAim, 100f, Object.InputAuthority, out var hit, collisionLayers, HitOptions.IncludePhysX);

        float hitDistance = 100f;
        bool isHitPlayer = false;

        if (hit.Distance > 0f)
            hitDistance = hit.Distance;

        if (hit.Hitbox != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit hitbox {hit.Hitbox.transform.root.name}");

            if (Object.HasStateAuthority)
                hit.Hitbox.transform.root.GetComponent<PlayerHealth>().OnTakeDamage(playerData.PlayerName.ToString());

            isHitPlayer = true;

        }
        else if (hit.Collider != null)
        {
            Debug.Log($"{Time.time} {transform.name} hit PhysX collider {hit.Hitbox.transform.root.name}");
        }

        if (isHitPlayer)
            Debug.DrawRay(aimPoint.position, forwardAim * hitDistance, Color.red, 1);
        else Debug.DrawRay(aimPoint.position, forwardAim * hitDistance, Color.green, 1);
        lastTimeFired = Time.time;
    }

    IEnumerator FireEffect()
    {
        isFiring = true;

        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }

    static void OnFireChanged(Changed<PlayerWeapon> changed)
    {

    }

    void OnFireRemote()
    {

    }
}
