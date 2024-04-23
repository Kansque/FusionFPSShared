using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    float mouseX, mouseY, horizontalInput, verticalInput;
    Rigidbody rb;
    LocalCamera cam;
    bool isGrounded, jumped = false;
    float speed = 7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<LocalCamera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void Update()
    {
        if(!HasInputAuthority)
            return;
        GetInput();
        CheckGrounded();
        LimitSpeed();
    }

    public override void  FixedUpdateNetwork()
    {
        if (!HasInputAuthority)
            return;
        MovePlayer();
    }

    private void GetInput()
    {
        

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        jumped = Input.GetKeyDown(KeyCode.Space);
    }

    private void MovePlayer()
    {
        transform.forward = cam.transform.forward;
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
        transform.rotation = rotation;

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.Normalize();

        
        Vector3 totalMove = speed * moveDirection;

        if (isGrounded)
        {
            rb.AddForce(totalMove, ForceMode.Impulse);
        }
        else if (isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            Mathf.Lerp(speed, 10f, Time.deltaTime * 5);
            rb.AddForce(totalMove, ForceMode.Impulse);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection * 0.1f, ForceMode.Impulse);
        }


        if (jumped && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.y);
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            jumped = false;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 3.64f * 0.5f + 0.2f);

        if (isGrounded)
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void LimitSpeed()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (currentVelocity.magnitude > 7f)
        {
            Vector3 limitedVelocity = currentVelocity.normalized * 7f;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }


}
