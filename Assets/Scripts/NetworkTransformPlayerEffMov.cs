using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class NetworkTransformPlayerEffMov : NetworkBehaviour {
    public float speed = 5f;

    public void Jump() {
        if(NetworkManager.Singleton.IsServer) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        } else {
            JumpServerRpc();
        }
    }

    [ServerRpc]
    void JumpServerRpc() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    void Update() {
        if(IsServer) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if(Input.GetButtonDown("Jump")) {
                Jump();
            }
            transform.position = transform.position + new Vector3(x, 0, z) * speed * Time.deltaTime;
        }
    }
}
