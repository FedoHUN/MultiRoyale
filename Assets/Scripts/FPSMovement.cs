using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FPSMovement : NetworkBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;
    [SerializeField] float sprintMultiplier = 1.5f;

    void Start() {
        rb = GetComponent<Rigidbody>();

        if(!isLocalPlayer) gameObject.layer = 11;
    }

    void Update() 
    {
        if(isLocalPlayer){
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 moveBy = transform.right * x + transform.forward * z;

            float actualSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift)) {
                actualSpeed *= sprintMultiplier;
            }

            rb.MovePosition(transform.position + moveBy.normalized * actualSpeed * Time.deltaTime);
        }
    }
}
