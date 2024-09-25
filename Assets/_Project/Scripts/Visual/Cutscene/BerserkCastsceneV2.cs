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

public class BerserkCastsceneV2 : MonoBehaviour
{
    [TabGroup("Castscene #1 - Close Exit")]
    [Title("GameObj Reference")]
    [SerializeField] List<GameObject> torchList1;

    [TabGroup("Castscene #1 - Close Exit")]
    [Title("Settings")]
    [SerializeField] float timeFadeParticleTorches;

    [TabGroup("Castscene #2 - Boss Intro")]
    [SerializeField] List<GameObject> torchList2;


    public void TochesFading()
    {
        foreach (var torch in torchList1)
        {
           torch.GetComponentInChildren<ParticleSystem>().Stop();
           torch.GetComponentInChildren<Animator>().Play("IronGateFeedback"); 
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

}

