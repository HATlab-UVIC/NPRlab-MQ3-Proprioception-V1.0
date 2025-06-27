using Unity.Netcode;
using UnityEngine;

public class NetworkManagerMetaQuest : MonoBehaviour
{
    private void Start() {
        NetworkManager.Singleton.StartClient();
    }
}
