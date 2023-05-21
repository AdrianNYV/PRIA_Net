using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class NetworkTransformPlayerEffMov : NetworkBehaviour {
    public float speed = 4f;

    private float buffAndDebuff = 1f;

    private MeshRenderer meshRenderer; 

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    [ServerRpc]
    void JumpServerRpc() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    [ServerRpc]
    void MoveServerRpc(Vector3 dir, float bd) {
        transform.position += dir * (speed + bd) * Time.deltaTime;
    }

    [ClientRpc]
    public void BuffOrDebuffClientRpc(float speed, float duration, Color color) {
        StartCoroutine(BuffOrDebuffCoroutine(speed, duration, color));
    } 

    IEnumerator BuffOrDebuffCoroutine(float speed, float duration, Color color) {
        float buffAndDebuffOriginal = this.buffAndDebuff;
        this.buffAndDebuff = speed;

        Color colorOriginal = meshRenderer.material.color;
        meshRenderer.material.color = color;

        yield return new WaitForSeconds(duration);

        this.buffAndDebuff = buffAndDebuffOriginal;
        meshRenderer.material.color = colorOriginal;
    }

    void Update() {
        if(IsOwner) {
            if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                MoveServerRpc(Vector3.right, buffAndDebuff);
            }
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                MoveServerRpc(Vector3.left, buffAndDebuff);
            }
            if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                MoveServerRpc(Vector3.forward, buffAndDebuff);
            }
            if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                MoveServerRpc(Vector3.back, buffAndDebuff);
            }
            if(Input.GetButtonDown("Jump")) {
                JumpServerRpc();
            }
        }
    }
}
