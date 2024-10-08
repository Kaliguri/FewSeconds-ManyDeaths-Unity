using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
public class InHero : NetworkBehaviour
{
    public int playerIDHost = 0;
    public NetworkVariable<int> ownerPlayerID = new NetworkVariable<int>(0);
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private GameObject heroSpritePrefab => playerInfoData.HeroDataList[ownerPlayerID.Value].GameObjectSpritePrefab;

    [SerializeField] private GameObject currentPrefab;

    private void Awake()
    {
        GlobalEventSystem.PlayerDied.AddListener(AmIDied);
    }

    void Start()
    {
        SetOridginSprite();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (NetworkManager.Singleton.IsHost)
        {
            ownerPlayerID.Value = playerIDHost;
        }
    }

    public void Transformation (GameObject graphicPrefab)
    {
        var NewSprite = Instantiate(graphicPrefab, transform);
        //Debug.Log("Love you :3");

        NewSprite.transform.SetParent(gameObject.transform);
        NewSprite.transform.position = currentPrefab.transform.position;
        NewSprite.transform.localScale = currentPrefab.transform.localScale;

        Destroy(currentPrefab);

        currentPrefab = NewSprite;

    }

    public void SetOridginSprite()
    {
        Transformation(heroSpritePrefab);
    }

    private void AmIDied(int playerID)
    {
        if (playerID == ownerPlayerID.Value)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}
