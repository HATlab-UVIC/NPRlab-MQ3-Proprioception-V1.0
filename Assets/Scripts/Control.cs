using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEditor;
using UnityEngine;

public class Control : NetworkBehaviour
{
    [SerializeField] private GameObject _prefab01;


    void Awake() {
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (clientId == NetworkManager.Singleton.LocalClientId) // only register for self
            {
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                    "HelloFromServer", OnHelloMessageReceived);
            }
        };
    }

    /*void Start()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
        {
            // Register a handler for "HelloMessage"
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(
                "HelloMessage", OnHelloMessageReceived);
        }
    }*/

    void Update()
    {
       
    }

    public void SpawnByNumber(int number) {
        switch (number)
        {
            case 1:
                var instance = Instantiate(_prefab01);
                NetworkDebugConsole.Singleton.SetDebugString("Prefab 01 instantiated locally");
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
        SendHelloToServer();
    }

    public void SendHelloToServer() {
        if (NetworkManager.Singleton.IsClient)
        {
            using var writer = new FastBufferWriter(128, Allocator.Temp);
            writer.WriteValueSafe(123);
            writer.WriteValueSafe(new FixedString64Bytes("Hi server!"));

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                "HelloFromClient",
                NetworkManager.ServerClientId,
                writer
            );
            NetworkDebugConsole.Singleton.SetDebugString("Trying to send Hello to server");
        }
    }
}
