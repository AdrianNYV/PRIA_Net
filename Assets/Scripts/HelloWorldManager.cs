using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld {
    public class HelloWorldManager : MonoBehaviour {
        private List<NetworkClient> listOfClientsIds = new List<NetworkClient>();

        private float buff = 2.5f;
        private float debuff = 0.5f;
        private float durationOffBD = 10f;

        private bool gameOver = false;

        void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
                StartButtons();
            } else {
                StatusLabels();
            }
            GUILayout.EndArea();
        }

        void StartButtons() {
            if (GUILayout.Button("Host")) {
                NetworkManager.Singleton.StartHost();
                StartCoroutine(BuffDebuff());
            }
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) {
                NetworkManager.Singleton.StartServer();
                StartCoroutine(BuffDebuff());
            }
        }

        void StatusLabels() {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        IEnumerator BuffDebuff() {
            while(!gameOver) {
                Debug.Log("Comienzo de la corutina");
                yield return new WaitForSeconds(20f);

                listOfClientsIds = new List<NetworkClient>();
                foreach(NetworkClient uid in NetworkManager.Singleton.ConnectedClientsList) {
                    listOfClientsIds.Add(uid);
                }

                int buffOrDebuff;
                buffOrDebuff = Random.Range(0, 2);
                int choosenClientId;
                choosenClientId = Random.Range(0, listOfClientsIds.Count);
                if(buffOrDebuff == 0) {
                    listOfClientsIds[choosenClientId].PlayerObject.GetComponent<NetworkTransformPlayerEffMov>().BuffOrDebuffClientRpc(buff, durationOffBD, Color.green);
                } else if(buffOrDebuff == 1) {
                    listOfClientsIds[choosenClientId].PlayerObject.GetComponent<NetworkTransformPlayerEffMov>().BuffOrDebuffClientRpc(debuff, durationOffBD, Color.red);
                }
            }
        }
    }
}