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

    private void Move() {
        if(NetworkManager.Singleton.IsServer) {
            
        } else {
            MoveServerRpc();
        }
    } 

    [ServerRpc]
    void MoveServerRpc() {

    }

    void Update() {
        if(IsServer) {
            if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) {
                Move();
            }
            if(Input.GetButtonDown("Jump")) {
                Jump();
            }
            transform.position = transform.position + new Vector3(0, 0, 0) * speed * Time.deltaTime;
        }
    }
}
