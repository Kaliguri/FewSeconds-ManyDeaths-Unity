using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class FeelFeedbacksManager : MonoBehaviour
{
    [Title("Feedbacks")]
    [Title("CameraShaking")]
    [SerializeField] MMF_Player cameraShakingLow;
    [SerializeField] MMF_Player cameraShakingMedium;
    [SerializeField] MMF_Player cameraShakingStrong;

    [Title("Slow-mo")]
    [SerializeField] MMF_Player slowMo;

    [Title("Post-Processing")]
    [SerializeField] MMF_Player postProcessing;

    public static FeelFeedbacksManager instance = null;
    void Awake()
    {
        instance = this;
    }
}