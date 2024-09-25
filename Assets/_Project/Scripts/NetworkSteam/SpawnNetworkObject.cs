using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VFavorites.Libs;

public class SpawnNetworkObject : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton.IsHost) GetComponent<NetworkObject>().Spawn();
    }
}
