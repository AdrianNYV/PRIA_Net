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

        static void StartButtons() {
            if (GUILayout.Button("Host")) {
                NetworkManager.Singleton.StartHost();
                StartCoroutine("IHopeYouFeelLuckyToday");
            }
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) {
                NetworkManager.Singleton.StartServer();
                StartCoroutine("IHopeYouFeelLuckyToday");
            }
        }

        static void StatusLabels() {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        IEnumerator IHopeYouFeelLuckyToday() {
            while(!gameOver) {
                Debug.Log("Comienzo de Corutina");
                yield return new WaitForSeconds(20f);
            }
        }
    }
}