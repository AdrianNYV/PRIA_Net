using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld {
    public class HelloWorldManager : MonoBehaviour {
        void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
                StartButtons();
            } else {
                StatusLabels();
                SubmitJump();
            }
            GUILayout.EndArea();
        }

        static void StartButtons() {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        static void StatusLabels() {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : "Client";
            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitJump() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Jump" : "Request Jump")) { 
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<NetworkTransformPlayerEffMov>();
                player.Jump();
            }
        }
    }
}