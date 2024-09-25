using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using VFavorites.Libs;
#endif

public class BerserkCastscene : MonoBehaviour
{
    [TabGroup("Castscene #1 - Close Exit")]
    [Title("Settings")]
    [SerializeField] float speedAnimation_1;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] float speedAppearanceCloseArea;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] float speedFadingIntensityTorch;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] float speedDisappearanceParticleTorch;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] float timeFadeParticleTorches;
    /*[TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] float intensityFadeTorchesModif = 0.5f; */

    [TabGroup("Castscene #1 - Close Exit")]
    [Title("GameObj Reference")]
    [SerializeField] GameObject ironGrating;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] List<GameObject> closeAreaList;
    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] GameObject torchObj;

    [TabGroup("Castscene #1 - Close Exit")]
    [SerializeField] List<GameObject> torchList1;

    [TabGroup("Castscene #1 - Close Exit")]
    [Title("Feedback Reference")]
    [SerializeField] MMF_Player ironGratingFeedback;

    [TabGroup("Castscene #2 - Boss Intro")]
    [SerializeField] List<GameObject> torchList2;


    void Awake()
    {
        GateEvent.GateClosed.AddListener(StartCastscene_1_Part_2);
        GateEvent.GateUp.AddListener(ironGratingUpFeedback);
    }

    [TabGroup("Castscene #1 - Close Exit")]
    [Button("Start Castscene #1")]
    public void StartCastscene_1_Part_1()
    {
        ironGrating.GetComponent<Animator>().enabled = true;
    }
    void StartCastscene_1_Part_2()
    {

        StartCoroutine(AppearanceCloseArea(closeAreaList[0]));
        StartCoroutine(AppearanceCloseArea(closeAreaList[1]));

        StartCoroutine(FadingIntensityTorch());
        
        DisappearanceParticleTorch();
    }

    void ironGratingUpFeedback()
    {
        ironGratingFeedback.PlayFeedbacks();

        TochesFading();

    }
    void TochesFading()
    {
        foreach (var torch in torchList1)
        {
           torch.GetComponentInChildren<ParticleSystem>().Stop();

           //torch.GetComponentInChildren<Light2D>().intensity *= intensityFadeTorchesModif;
        }
        Invoke("TochesBurn", timeFadeParticleTorches);
    }
    void TochesBurn()
    {
        foreach (var torch in torchList1)
        {
           torch.GetComponentInChildren<ParticleSystem>().Play();

           //torch.GetComponentInChildren<Light2D>().intensity /= intensityFadeTorchesModif;
        }
        
    } 

    IEnumerator AppearanceCloseArea(GameObject area)
    {
        var color = area.GetComponent<Tilemap>().color;

        while (color.a < 1)
        {
            color.a += speedAppearanceCloseArea;
            area.GetComponent<Tilemap>().color = color;

            yield return new WaitForSeconds(1 / speedAnimation_1);
        }

    }

    IEnumerator FadingIntensityTorch()
    {
        var lightVolume = torchObj.GetComponentInChildren<Light2D>().intensity;
        
        while (lightVolume > 0)
        {
            lightVolume -= speedFadingIntensityTorch;
            torchObj.GetComponentInChildren<Light2D>().intensity = lightVolume;

            yield return new WaitForSeconds(1 / speedAnimation_1);
        }
        Destroy(torchObj);       
    }
    void DisappearanceParticleTorch()
    {
        var particleSystem = torchObj.GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop();
        //var particle = torchObj.GetComponentInChildren<ParticleExtensions>();
        //StartCoroutine(particle.DescreaseTransparencyParticles(speedDisappearanceParticleTorch, 1 / speedAnimation_1));
    }



    

}

