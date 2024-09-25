using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldInfoManager : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] Image shieldIcon;
    [SerializeField] TextMeshProUGUI shieldText;

    [Title("Settings")]
    [SerializeField] float renderChangeValue = 0.5f;

    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private int UINumber;

    private float shieldValue;
    

    void Awake()
    {
        GlobalEventSystem.PlayerShieldChanged.AddListener(shieldTextUpdate);
        GlobalEventSystem.CombatPlayerDataInStageInitialized.AddListener(shieldTextUpdate);
    }

    void Start()
    {
        UINumber = GetComponentInParent<PlayerInfoVisual>().UINumber;
    }

    void shieldTextUpdate()
    {
        shieldValue = combatPlayerDataInStage._TotalStatsList[UINumber].currentCombat.CurrentShield;

        if (shieldValue > 0) { ShowShieldInfo(); }
        else                  { HideShieldInfo(); }
        
    }

    void ShowShieldInfo()
    {
        shieldText.text = shieldValue.ToString();
        RenderChange(true);
    }
    void HideShieldInfo()
    {
        shieldText.text = "";
        RenderChange(false);
        
    }

    void RenderChange(bool activeBool)
    {
        var tempColor = shieldIcon.color;

        if (activeBool) { shieldIcon.color = new Color(tempColor.r,tempColor.g,tempColor.b, 1); }
        else            { shieldIcon.color = new Color(tempColor.r,tempColor.g,tempColor.b, renderChangeValue); }
    }


    [Title ("Armor Apply Test")]
    public float Shield = 20f;
    [Button ("ShieldApply!")]
    public void DamageApply()
    {
        combatPlayerDataInStage._TotalStatsList[UINumber].currentCombat.CurrentShield += Shield;
        GlobalEventSystem.SendPlayerShieldChanged();
    }
}
