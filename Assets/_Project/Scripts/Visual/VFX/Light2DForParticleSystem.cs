﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class Light2DForParticleSystem : MonoBehaviour
{
    [SerializeField] GameObject Light2D_Prefab;
    [SerializeField]  float intensityScale = 1f;

    private GameObject _light2D;
    private Light2D _light2DComponent;


    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        Light2D_Prefab.SetActive(false);

        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
        {
            _light2D = Instantiate(Light2D_Prefab, m_ParticleSystem.transform);
            _light2D.SetActive(true);
            m_Instances.Add(_light2D);
        }   

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);

                //Debug.Log("Old: " + m_Instances[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity);

                _light2DComponent = m_Instances[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                _light2DComponent.intensity = (m_Particles[i].GetCurrentSize(m_ParticleSystem) * intensityScale);

                //Debug.Log("New: " + m_Instances[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
