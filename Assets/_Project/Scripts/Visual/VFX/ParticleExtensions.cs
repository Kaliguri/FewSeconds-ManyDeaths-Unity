using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleExtensions : MonoBehaviour
{
    private ParticleSystem _particleSystem => GetComponent<ParticleSystem>();
    private ParticleSystem.Particle[] _particles => new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    private int particleCount => _particleSystem.GetParticles(_particles);

    public IEnumerator DescreaseTransparencyParticles(float effect, float rateChanges)
    {
        if (particleCount != 0)
        {
            while (_particles[particleCount].startColor.a <= 0)
            {
                for (int p = 0; p < _particles.Length; p++)
                {    
                    Color color = _particles[p].startColor;
                    color.a -= effect;

                    _particles[p].startColor = color;
                }
                
            //Debug.Log(_particles[particleCount].startColor.a);    
            yield return new WaitForSeconds(rateChanges);
            }
        }
    }
}
