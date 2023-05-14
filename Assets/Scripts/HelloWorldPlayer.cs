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

        public NetworkVariable<int> PlayerIdColor;

        public List<Color> colors = new List<Color>();

        private void Start() {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                Move();
                Colorized();
            }
        }

        //Coloreador de jugadores
        public void Colorized() {
            int idForColor;
            if (NetworkManager.Singleton.IsServer) {
                idForColor = ColorAccept();
                meshRenderer.material.color = colors[idForColor];
                PlayerIdColor.Value = idForColor;
            } else {
                SumbitColorizedResquestServerRpc();
            }
        }

        [ServerRpc]
        void SumbitColorizedResquestServerRpc() {
            int idForColor = ColorAccept();
            PlayerIdColor.Value = idForColor;
        }

        int ColorAccept() {
            int idColor;
            bool sameColor;
            List<int> colorsInUse = new List<int>();
            foreach(ulong uid in NetworkManager.Singleton.ConnectedClientsIds) {
                colorsInUse.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().PlayerIdColor.Value);
            } do {
                idColor = Random.Range(0, colors.Count);
                sameColor = colorsInUse.Contains(idColor);
            } while (sameColor);
            return idColor;
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

        //Botón [Move] de Teleport del Player
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

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane() {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update(){
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Colorized();
                }
            }
            transform.position = Position.Value;

            
            //Comprobación para no hacer asignación de color en cada frame, sino cuando el valor cambie asignar el nuevo color
            if (meshRenderer.material.color != colors[PlayerIdColor.Value]) {
                meshRenderer.material.color = colors[PlayerIdColor.Value];
            }
        }
    }
}