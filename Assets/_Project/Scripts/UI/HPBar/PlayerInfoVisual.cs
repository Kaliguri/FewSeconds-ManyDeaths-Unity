using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class PlayerInfoVisual : MonoBehaviour
{

    [Title("Gameobject Reference")]
    [SerializeField] TextMeshProUGUI nicknameObj;
    [SerializeField] Image icon;

    [Title("Other")]
    public int UINumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerCount => playerInfoData.PlayerCount;
    private string nickname => playerInfoData.NicknameList[UINumber];
    private Sprite heroIcon => playerInfoData.HeroDataList[UINumber].HeroIcon;

    void Awake()
    {
        GlobalEventSystem.CombatPlayerDataInStageInitialized.AddListener(DeactivateOverUI);
    }
    public void Inizialize()
    {
        DataTransfer();
    }

    void DeactivateOverUI()
    {
        if   ( playerCount >= UINumber + 1) 
        {
            Inizialize();
        }
        else { gameObject.SetActive(false);  }  //Removing unnecessary UI
    }
    void DataTransfer()
    {
        nicknameObj.text = nickname;
        icon.sprite = heroIcon;
    }


}

