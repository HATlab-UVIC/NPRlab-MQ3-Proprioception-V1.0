using System;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkDebugConsole : MonoBehaviour
{
    public static NetworkDebugConsole Singleton { get; private set; }
    public enum ConnectionStatus {
        Connected,
        Disconnected,
    }
    public event Action<ulong, ConnectionStatus> OnClientConnection;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private int _lineCountMax = 19;
    private ulong hostId;
    private int _lineCount = 0;

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
        else if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            SetDebugString("Local client disconnected");
        }
        else
        {
            SetDebugString($"Another client disconnected with {clientId}");
        }
    }

    public void SetDebugString(string str) {
        string[] lines = _tmpText.text.Split(new[] { '\n' }, StringSplitOptions.None);
        if (_lineCount > 0)
        {
            if (_lineCount >= _lineCountMax)
            {
                int index = _tmpText.text.IndexOf(System.Environment.NewLine);
                _tmpText.text = string.Join("\n", lines.Skip(1)); ;
                _lineCount--;
            }
        }
        _tmpText.text += DateTime.Now.ToString("HH:mm:ss") + " " + str + "\n";
        _lineCount++;
        Debug.Log(_tmpText.text);
    }

}
