using Unity.VisualScripting;
using UnityEngine;
public class InHero : MonoBehaviour
{
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private GameObject heroSpritePrefab => playerInfoData.HeroDataList[playerID].GameObjectSpritePrefab;

    [SerializeField] private GameObject currentPrefab;

    void Start()
    {
        SetOridginSprite();
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
}
