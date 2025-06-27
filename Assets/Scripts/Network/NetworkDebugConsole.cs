using System;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetworkDebugConsole : MonoBehaviour
{
    public static NetworkDebugConsole Singleton { get; private set; }
    public enum ConnectionStatus {
        Connected,
        Disconnected,
    }
    public event Action<ulong, ConnectionStatus> OnClientConnection;
    [SerializeField] private TMP_Text   _tmpText;
    private ulong hostId;

    private void Awake() {
        if (Singleton != null)
        {
            throw new Exception($"Detected more than one instance of {nameof(NetworkDebugConsole)}! " +
                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }

    private void Start() {
        if (Singleton != this)
        {
            return;
        }
        if (NetworkManager.Singleton == null)
        {
            throw new Exception($"There is no {nameof(NetworkManager)} for the {nameof(NetworkDebugConsole)} to do stuff with! " +
                $"Please add a {nameof(NetworkManager)} to the scene.");
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnNetworkConnectionEvent;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnNetworkDisconnectionEvent;
    }

    private void OnDestroy() {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnNetworkConnectionEvent;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnNetworkDisconnectionEvent;
        }
    }

    private void OnNetworkConnectionEvent(ulong clientId) {
        OnClientConnection?.Invoke(clientId, ConnectionStatus.Connected);
        if (NetworkManager.Singleton.IsHost)
        {
            SetDebugString("Hosted with id: " + clientId);
            hostId = clientId;
        }
        else if (NetworkManager.Singleton.ConnectedClientsIds.Contains(clientId))
        {
            SetDebugString("Client connected with id: " + clientId);
        }
    }
    private void OnNetworkDisconnectionEvent(ulong clientId) {
        OnClientConnection?.Invoke(clientId, ConnectionStatus.Disconnected);
        if (clientId == hostId)
        {
            SetDebugString("Host disconnected with host id: " + clientId);
        }
        else
        {
            SetDebugString("Disconnected for unknown reason");
        }
    }

    private void SetDebugString(string str) {
        _tmpText.text += DateTime.Now.ToString("HH:mm:ss") + " " + str + "\n";
    }

}
