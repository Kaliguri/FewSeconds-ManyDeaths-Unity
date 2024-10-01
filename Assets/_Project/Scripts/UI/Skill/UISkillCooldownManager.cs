using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UISkillCooldownManager : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] TextMeshProUGUI cooldownValueText;

    private SkillCooldownManager skillCooldownManager => FindObjectOfType<SkillCooldownManager>();
    private int UINumber;
    private int PlayerID => FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;

    void Awake()
    {
        GlobalEventSystem.UpdateCooldown.AddListener(UpdateCooldown);
        
    }
    void Start()
    {
        UINumber = gameObject.GetComponentInParent<UISkillV3>().UINumber; 
        gameObject.SetActive(false);
    }

    void UpdateCooldown()
    {
        var cooldownValue = skillCooldownManager.GetSkillCooldown(PlayerID, UINumber);

        if (cooldownValue != 0)
        {
            gameObject.SetActive(true);
            cooldownValueText.text = cooldownValue.ToString();
        }

        else
        {
            gameObject.SetActive(false);
        }
    }
}
