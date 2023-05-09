using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public static float speed = 3f;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
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

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return Vector3.zero;
        }

        void Update()
        {
            if(Input.GetKey(KeyCode.UpArrow)) {
                transform.position += speed * Vector3.forward * Time.deltaTime;
            } else if(Input.GetKey(KeyCode.DownArrow)) {
                transform.position += speed * Vector3.back * Time.deltaTime;
            } else if(Input.GetKey(KeyCode.LeftArrow)) {
                transform.position += speed * Vector3.left * Time.deltaTime;
            } else if(Input.GetKey(KeyCode.RightArrow)) {
                transform.position += speed * Vector3.right * Time.deltaTime;
            } else {
                transform.position += Vector3.zero;
            }
        }
    }
}