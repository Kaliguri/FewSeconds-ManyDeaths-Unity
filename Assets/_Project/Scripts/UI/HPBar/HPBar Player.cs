using UnityEngine;
using UnityEngine.UI;

public class HPBarPlayer : MonoBehaviour
{
    [SerializeField] float _lerpSpeed = 0.08f;
    [SerializeField] Slider _secondarySlider;
    [SerializeField] Slider _primarySlider;
    
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private float _maxHeatlh => combatPlayerDataInStage._TotalStatsList[playerID].general.MaxHP;
    private float _currentHeatlh => combatPlayerDataInStage._TotalStatsList[playerID].currentCombat.CurrentHP;
    // Start is called before the first frame update
    // Update is called once per frame
    void Start()
    {
        _primarySlider.maxValue = _maxHeatlh;
        _secondarySlider.maxValue = _maxHeatlh;
    }
    void Update()
    {
        if (_primarySlider.value != _currentHeatlh)
        {
            _primarySlider.value = _currentHeatlh;
        }

        if (_primarySlider.value != _secondarySlider.value)
        {
            _secondarySlider.value = Mathf.Lerp(_secondarySlider.value, _currentHeatlh, _lerpSpeed);
        }

    }
}