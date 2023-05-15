using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class NetworkTransformPlayerEffMov : NetworkBehaviour {
    public float speed = 5f;

    void Update() {
        if(IsServer) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            transform.position = transform.position + new Vector3(x, 0, z) * speed * Time.deltaTime;
        }
    }
}
