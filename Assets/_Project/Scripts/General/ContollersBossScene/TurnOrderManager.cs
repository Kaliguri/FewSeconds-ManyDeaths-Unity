using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TurnOrderManager : NetworkBehaviour
{
    [SerializeField] List<GameObject> playersIconsList = new List<GameObject>();

    private void Awake()
    {
        GlobalEventSystem.PredictionStageStarted.AddListener(RandomiseTurnOrder);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    void Start()
    {
        for (int i = 0; i < playersIconsList.Count; i++)
        {
            if (i >= PlayerInfoData.instance.PlayerCount) playersIconsList[i].SetActive(false);
            else playersIconsList[i].GetComponentInChildren<Image>().sprite = PlayerInfoData.instance.HeroDataList[i].HeroIcon;
        }
    }

    private void RandomiseTurnOrder()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            List<int> newOrder = CombatPlayerDataInStage.instance.TurnPriority.ToList();
            RandomiseList(newOrder);

            SetNewTurnOrderRpc(newOrder.ToArray());
        }
    }

    private void RandomiseList(List<int> list)
    {
        System.Random rng = new System.Random();
        int elementIndex = PlayerInfoData.instance.PlayerCount;
        while (elementIndex > 1)
        {
            elementIndex--;
            int randomElementIndex = rng.Next(elementIndex + 1);
            (list[elementIndex], list[randomElementIndex]) = (list[randomElementIndex], list[elementIndex]);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SetNewTurnOrderRpc(int[] newTurnOrder)
    {
        CombatPlayerDataInStage.instance.TurnPriority = newTurnOrder.ToList();
        for (int i = 0; i < newTurnOrder.Length; i++) 
        { 
            playersIconsList[i].GetComponentInChildren<Image>().sprite = PlayerInfoData.instance.HeroDataList[newTurnOrder[i]].HeroIcon;
            Debug.Log("Player " + newTurnOrder[i] + " is gonna cast " + (i + 1));
        }
    }
}
