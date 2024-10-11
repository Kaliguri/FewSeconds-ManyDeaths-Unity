using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;

    }
    public void OnParticleSystemStopped()
    {
        Destroy(gameObject.transform.parent.gameObject);
        //Debug.Log("Destroy");
    }
}
