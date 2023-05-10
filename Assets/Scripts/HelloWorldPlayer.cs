using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        
        [SerializeField]
        private MeshRenderer meshRenderer;

        public List<Color> colors = new List<Color>();

        public void Awake() {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
            meshRenderer.material.color = colors[(int)OwnerClientId];
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        //Movimiento hacia delante
        public void MoveForward() {
            if (NetworkManager.Singleton.IsServer) {
                Position.Value += Vector3.forward;
            } else {
                SubmitForwardPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitForwardPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value += Vector3.forward;
        }

        //Movimiento hacia atrás
        public void MoveBack() {
            if (NetworkManager.Singleton.IsServer) {
                Position.Value += Vector3.back;
            } else {
                SubmitBackPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitBackPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value += Vector3.back;
        }

        //Movimiento hacia la izquierda
        public void MoveLeft() {
            if (NetworkManager.Singleton.IsServer) {
                Position.Value += Vector3.left;
            } else {
                SubmitLeftPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitLeftPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value += Vector3.left;
        }

        //Movimiento hacia la derecha
        public void MoveRight() {
            if (NetworkManager.Singleton.IsServer) {
                Position.Value += Vector3.right;
            } else {
                SubmitRightPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitRightPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value += Vector3.right;
        }

        //Botón [Move]
        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            if(IsOwner) {
                if(Input.GetKeyDown(KeyCode.UpArrow)) {
                    MoveForward();
                }     
                if(Input.GetKeyDown(KeyCode.DownArrow)) {
                    MoveBack();
                }             
                if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    MoveLeft();
                }             
                if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    MoveRight();
                }  
            }
            transform.position = Position.Value;
        }
    }
}