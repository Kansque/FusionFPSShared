using System.Collections;
using System.Collections.Generic;
using Fusion;
using StarterAssets;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    bool isInitialized = false;

    const byte initialHP = 5;

    public Color uiOnHitColor;
    //public Image uiOnHitImage;

    public MeshRenderer meshRenderer;
    Color defaultMeshColor;

    public GameObject playerBody;
    public HitboxRoot hitboxRoot;
    //NetworkInGameMessages networkInGameMessages;
    NetworkPlayer networkPlayer;

    ThirdPersonController cc;
    private void Awake()
    {
        cc = GetComponent<ThirdPersonController>();
        networkPlayer = GetComponent<NetworkPlayer>();
        //networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }
    void Start()
    {
        HP = initialHP;
        isDead = false;

        defaultMeshColor = meshRenderer.material.color;

        isInitialized = true;
    }

    IEnumerator OnHit()
    {
        meshRenderer.material.color = Color.white;

        //if (Object.HasInputAuthority)
            //uiOnHitImage.color = uiOnHitColor;

        yield return new WaitForSeconds(0.2f);

        meshRenderer.material.color = defaultMeshColor;

        //if (Object.HasInputAuthority && !isDead)
           // uiOnHitImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator ServerRevive()
    {
        yield return new WaitForSeconds(2f);
        //cc.RequestRespawn();
    }

    public void OnTakeDamage(string whoDamaged)
    {
        if (isDead) return;

        HP -= 1;

        Debug.Log($"{Time.time} {transform.name} took damage got {HP} left ");

        if (HP <= 0)
        {
            //networkInGameMessages.SendInGameRPCMessage(whoDamaged, $"Killed <b>{networkPlayer.playerName.ToString()}</b>");
            Debug.Log($"{Time.time} {transform.name} died");


            isDead = true;

            StartCoroutine(ServerRevive());


        }
    }

    static void OnHPChanged(Changed<PlayerHealth> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.HP}");

        byte newHP = changed.Behaviour.HP;

        changed.LoadOld();

        byte oldHP = changed.Behaviour.HP;

        if (newHP < oldHP)
        {
            changed.Behaviour.OnHPReduced();
        }
    }

    private void OnHPReduced()
    {
        if (!isInitialized)
            return;

        StartCoroutine(OnHit());
    }

    static void OnStateChanged(Changed<PlayerHealth> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.isDead}");

        bool isCurrentlyDead = changed.Behaviour.isDead;
        changed.LoadOld();

        bool isDeadOld = changed.Behaviour.isDead;

        if (isCurrentlyDead)
            changed.Behaviour.OnDeath();
        else if (!isCurrentlyDead && isDeadOld)
            changed.Behaviour.Revive();
    }

    private void OnDeath()
    {
        playerBody.gameObject.SetActive(false);
        hitboxRoot.HitboxRootActive = false;
        //cc.SetCharacterControllerEnabled(false);

    }

    private void Revive()
    {
        if (Object.HasInputAuthority)
            //uiOnHitImage.color = new Color(0, 0, 0, 0);

        playerBody.gameObject.SetActive(true);
        hitboxRoot.HitboxRootActive = true;
        //cc.SetCharacterControllerEnabled(true);
    }

    public void OnRespawn()
    {
        HP = initialHP;
        isDead = false;
    }
}
