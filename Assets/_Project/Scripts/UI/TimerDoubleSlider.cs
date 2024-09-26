using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TimerDoubleSlider : MonoBehaviour
{
    [Title("Gameobjects Reference")]
    [Title("Left->Right Slider")]
    [SerializeField] GameObject leftSlider;

    [Title("Right->Left Slider")]
    [SerializeField] GameObject rightSlider;

    private float turnTime => FindObjectOfType<CombatStageManager>().PlayerTurnTime;

    void Awake()
    {
        GlobalEventSystem.PlayerTurnStageTimerUpdate.AddListener(UpdateValue);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(StartPlayerTurnStage);
        GlobalEventSystem.PlayerTurnStageEnded.AddListener(EndPlayerTurnStage);
    }

    void Start()
    {
        ValueTransfer(turnTime);
        gameObject.SetActive(false);
    }

    public void ValueTransfer(float maxValue)
    {
        leftSlider.GetComponent<Slider>().maxValue = maxValue;
        leftSlider.GetComponent<Slider>().value = maxValue;

        rightSlider.GetComponent<Slider>().maxValue = maxValue;
        rightSlider.GetComponent<Slider>().value = maxValue;
    }

    public void UpdateValue(float timeValue)
    {
        leftSlider.GetComponent<Slider>().value = timeValue;
        rightSlider.GetComponent<Slider>().value = timeValue;
    }

    private void StartPlayerTurnStage()
    {
        gameObject.SetActive(true);
    }

    private void EndPlayerTurnStage()
    {
        gameObject.SetActive(false);
    }
}
