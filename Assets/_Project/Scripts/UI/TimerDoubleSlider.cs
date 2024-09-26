using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TimerDoubleSlider : MonoBehaviour
{
    [Title("Gameobjects Reference")]
    [Title("Left->Right Slider")]
    [SerializeField] GameObject leftSlider;
    [SerializeField] Image leftSliderImage;

    [Title("Right->Left Slider")]
    [SerializeField] GameObject rightSlider;
    [SerializeField] Image rightSliderImage;

    [Title("Visual")]
    [SerializeField] Sprite sliderImage; 

    void Awake()
    {
        GlobalEventSystem.PlayerTurnStageTimerUpdate.AddListener(UpdateValue);
    }

    void Start()
    {
        ImageTransfer();
    }

    void ImageTransfer()
    {
        leftSliderImage.sprite = sliderImage;
        rightSliderImage.sprite = sliderImage;
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
}
