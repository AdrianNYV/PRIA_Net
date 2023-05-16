using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class NetworkTransformPlayerEffMov : NetworkBehaviour {
    public float speed = 5f;

    public void Jump() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
    } 

    void Update() {
        if(IsOwner) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if(Input.GetButtonDown("Jump")) {
                Jump();
            }
            transform.position = transform.position + new Vector3(x, 0, z) * speed * Time.deltaTime;
        }
    }
}
