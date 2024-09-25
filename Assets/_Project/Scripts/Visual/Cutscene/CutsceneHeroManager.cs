using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CutsceneHeroManager : MonoBehaviour
{
    [Title("Hero Objects")]
    [SerializeField] List<GameObject> heroGameobjectList;

    [Title("Settings")]
    [SerializeField] bool IsWithoutDependenceOnPlayerCount;
    [SerializeField] bool IsWithoutPlayerInfoData;

    [Title("Without PlayerInfoData Settings")]
    [ShowIf("IsWithoutPlayerInfoData")]
    [SerializeField] int maxPlayer;

    [ShowIf("IsWithoutPlayerInfoData")]
    [SerializeField] List<HeroData> heroDataList;
    private PlayerInfoData playerInfoData;

    void Start()
    {
        if (!IsWithoutPlayerInfoData) {playerInfoData = FindObjectOfType<PlayerInfoData>();}

        DataTransfer();

        if (!IsWithoutDependenceOnPlayerCount)
        {
            if (!IsWithoutPlayerInfoData) {DeactivateSpriteMissingPlayers(playerInfoData.PlayerCount); }
            else {DeactivateSpriteMissingPlayers(maxPlayer);}
        }
    }
    void DataTransfer()
    {
        
        if (!IsWithoutPlayerInfoData) {HeroSpriteDataTransfer( playerInfoData.HeroDataList); }
        else {HeroSpriteDataTransfer(heroDataList);}
    }
    void HeroSpriteDataTransfer(List<HeroData> heroDataList)
    {
        for (int i = 0; i < heroDataList.Count; i++)
        {
            GameObject tempSprite = heroGameobjectList[i].GetComponentInChildren<SpriteRenderer>().gameObject;
            Destroy(tempSprite);

            GameObject sprite = Instantiate(heroDataList[i].GameObjectSpritePrefab);
            sprite.transform.SetParent(heroGameobjectList[i].transform);
            sprite.transform.localPosition = new Vector3(0,0,0);
        }

    }
    void DeactivateSpriteMissingPlayers(int playerCount)
    {
        for (int i = 0; i < heroGameobjectList.Count; i++)
        {
            if (i+1 > playerCount)
            {
                heroGameobjectList[i].SetActive(false);
            }
        }
    }

}
