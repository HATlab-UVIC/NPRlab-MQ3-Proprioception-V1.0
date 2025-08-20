using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEditor;
using UnityEngine;

public class ControlManager : NetworkBehaviour
{
    public static ControlManager Singleton { get; private set; }
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private float _pivotDistance = 0.2f;
    [SerializeField] private float _pivotScale = 0.2f;
    private Vector3 _pivotPosition;


    void Awake() {
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (clientId == NetworkManager.Singleton.LocalClientId) // only register for self
            {
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                    "HelloFromServer", OnHelloMessageReceived);
            }
        };
        if (Singleton != null)
        {
            throw new Exception($"Detected more than one instance of {nameof(NetworkDebugConsole)}! " +
                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }

    void Start() {
        _pivotPosition = new Vector3(-_pivotScale, _pivotScale, 0);
    }

    void Update()
    {
       
    }

    public void SpawnByNumber(int number) {
        GameObject instance;
        number -= 1;
        switch (number)
        {
            case 0:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                instance.GetComponent<TargetController>().index = number;
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                break;
            case 1:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 2:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 3:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 4:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 5:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 6:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 7:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            case 8:
                instance = Instantiate(_targetPrefab);
                instance.transform.position = _pivotPosition + new Vector3((number % 3) * _pivotScale, -(number / 3) * _pivotScale, _pivotDistance);
                NetworkDebugConsole.Singleton.SetDebugString($"Prefab {number + 1} instantiated locally");
                instance.GetComponent<TargetController>().index = number;
                break;
            default:
                NetworkDebugConsole.Singleton.SetDebugString("Not a valid number input");
                break;
        }
    }

    private void OnHelloMessageReceived(ulong senderClientId, FastBufferReader reader) {
        // Read payload in same order as server wrote it
        reader.ReadValueSafe(out int number);
        reader.ReadValueSafe(out FixedString64Bytes text);

        Debug.Log($"[Client] Received from {senderClientId}: {number}, {text}");
        NetworkDebugConsole.Singleton.SetDebugString($"Received from {senderClientId}: {number}, {text}");
        SpawnByNumber(number);
        SendHelloToServer(number);
    }

    public void SendHelloToServer(int number) {
        if (NetworkManager.Singleton.IsClient)
        {
            using var writer = new FastBufferWriter(128, Allocator.Temp);
            writer.WriteValueSafe(number);
            writer.WriteValueSafe(new FixedString64Bytes("Hi server!"));

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                "HelloFromClient",
                NetworkManager.ServerClientId,
                writer
            );
        }
    }

    public void SendCaptureToServer(int number) {
        if (NetworkManager.Singleton.IsClient)
        {
            using var writer = new FastBufferWriter(128, Allocator.Temp);
            writer.WriteValueSafe(number);
            writer.WriteValueSafe(new FixedString64Bytes("Captured"));

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                "CaptureFromClient",
                NetworkManager.ServerClientId,
                writer
            );
        }
    }
}
