using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovePlayer : NetworkBehaviour {
    private float speed = 8f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start() {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        if(IsServer) {
            Vector3 tmpPosition = transform.position;
            tmpPosition += velocity * Time.deltaTime;
            transform.position = tmpPosition;

            if(Input.GetKey(KeyCode.UpArrow)) {
                velocity = speed * Vector3.forward;
            } else if(Input.GetKey(KeyCode.DownArrow)) {
                velocity = speed * Vector3.back;
            } else if(Input.GetKey(KeyCode.LeftArrow)) {
                velocity = speed * Vector3.left;
            } else if(Input.GetKey(KeyCode.RightArrow)) {
                velocity = speed * Vector3.right;
            } else {
                velocity = Vector3.zero;
            }
        }
    }
}
