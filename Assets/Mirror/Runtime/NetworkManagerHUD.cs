// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        [SerializeField] GameObject dijalog;
        [SerializeField] Button hostDugme;
        [SerializeField] Button klijentDugme;
        [SerializeField] InputField nadimakInput;
        [SerializeField] InputField hostAdresaInput;

        NetworkManager manager;
        public static int broj = 1;
        string nadimak = "";

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        public void Povezi(bool jeHost)
        {
            if (jeHost)
            {
                manager.StartHost();
                PlayerPrefs.SetString("igrac1", this.nadimakInput.text);
            }
            else
            {
                manager.StartClient();
                manager.networkAddress = this.hostAdresaInput.text;
                PlayerPrefs.SetString("igrac2", this.nadimakInput.text);
            }

            this.dijalog.GetComponent<Animator>().enabled = true;
        }

        void OnGUI()
        {
            //GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            //if (!NetworkClient.isConnected && !NetworkServer.active)
            //{
            //    StartButtons();

            //    GUILayout.BeginHorizontal();
            //    {
            //        GUILayout.Label("Unesite nadimak:");
            //        this.nadimak = GUILayout.TextField(this.nadimak, 10);
            //    }
            //    GUILayout.EndHorizontal();
            //}
            ////else
            ////{
            ////    StatusLabels();
            ////}

            //// client ready
            //if (NetworkClient.isConnected && !NetworkClient.ready)
            //{
            //    if (GUILayout.Button("Client Ready"))
            //    {
            //        NetworkClient.Ready();
            //        if (NetworkClient.localPlayer == null)
            //        {
            //            NetworkClient.AddPlayer();
            //        }
            //    }
            //}

            //StopButtons();

            //GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server i Klijent)"))
                    {
                        manager.StartHost();
                        PlayerPrefs.SetString("igrac1", this.nadimak);
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Klijent"))
                {
                    manager.StartClient();
                    PlayerPrefs.SetString("igrac2", this.nadimak);
                }
                // This updates networkAddress every frame from the TextField
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                //else
                //{
                //    if (GUILayout.Button("Server Only")) manager.StartServer();
                //}
            }
            else
            {
                // Connecting
                GUILayout.Label($"Povezujem se na {manager.networkAddress}..");
                if (GUILayout.Button("Otkazi pokusaj povezivanja"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Izadji iz igre"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Izadji iz igre"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
    }
}
