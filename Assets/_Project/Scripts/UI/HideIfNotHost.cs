using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HideIfNotHost : MonoBehaviour
{
    [SerializeField] bool forHost = true;
    void Start()
    {
        if (!NetworkManager.Singleton.IsHost && forHost) gameObject.SetActive(false);
        else if (!forHost) gameObject.SetActive(false);
    }
}
