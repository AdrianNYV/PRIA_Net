using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class NetworkTransformPlayerEffMov : NetworkBehaviour {
    public float speed = 5f;

    [ServerRpc]
    void JumpServerRpc() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    [ServerRpc]
    void MoveRightServerRpc(Vector3 dirRight) {
        transform.position += dirRight * speed * Time.deltaTime;
    }

    [ServerRpc]
    void MoveLeftServerRpc(Vector3 dirLeft) {
        transform.position += dirLeft * speed * Time.deltaTime;
    }

    [ServerRpc]
    void MoveForwardServerRpc(Vector3 dirForward) {
        transform.position += dirForward * speed * Time.deltaTime;
    }

    [ServerRpc]
    void MoveBackServerRpc(Vector3 dirBack) {
        transform.position += dirBack * speed * Time.deltaTime;
    }

    void Update() {
        if(IsOwner) {
            if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                MoveRightServerRpc(Vector3.right);
            }
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                MoveLeftServerRpc(Vector3.left);
            }
            if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                MoveForwardServerRpc(Vector3.forward);
            }
            if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                MoveBackServerRpc(Vector3.back);
            }
            if(Input.GetButtonDown("Jump")) {
                JumpServerRpc();
            }
        }
    }
}
