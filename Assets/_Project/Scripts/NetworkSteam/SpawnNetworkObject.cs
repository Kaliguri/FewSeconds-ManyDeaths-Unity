
using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkObject : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton.IsHost) GetComponent<NetworkObject>().Spawn();
    }
}
