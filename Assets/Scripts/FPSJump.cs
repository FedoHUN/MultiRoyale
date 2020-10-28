using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FPSJump : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundChecker;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool IsOnGround() 
        {
            Collider[] colliders = Physics.OverlapSphere(groundChecker.position, checkRadius, groundLayer);
            if (colliders.Length > 0) 
            {
                return true;
            }else 
            {
                return false;
            }
        }

    if (Input.GetKeyDown(KeyCode.Space) && IsOnGround()) 
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    }
}