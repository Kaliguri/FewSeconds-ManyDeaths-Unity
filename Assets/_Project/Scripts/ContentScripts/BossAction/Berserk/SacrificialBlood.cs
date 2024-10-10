using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[Serializable]
public class SacrificialBlood : BossActionScript
{
    [Title("Stats")]
    [SerializeField] private int damage = 20;


    [Title("Visual")]
    [SerializeField] private int circleBloodRadius = 2;
    [SerializeField] private float circlingTime = 2f;
    [SerializeField] private float betweenSendTime = 0.5f;
    [SerializeField] private float startCirclingTime = 1f;
    [SerializeField] private float particleMoveTime = 1f;
    [SerializeField] private Vector2 centerModification = new Vector2( 0f, 0.5f);


    [Title("Prefabs")]
    [SerializeField] private GameObject bloodParticlePrefab;

    [Title("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;
    

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);

        Debug.Log("Cast Berserk Sacrificial Blood!");

        CastSacrificialBlood();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return new List<Vector2> { GetFarthestHero() };
    }

    private Vector2 GetFarthestHero()
    {
        List<Vector2> heroCoordinates = combatPlayerDataInStage.HeroCoordinates.ToList();
        List<bool> aliveStatus = combatPlayerDataInStage.aliveStatus.ToList();

        Vector2 farthestHero = bossManager.CurrentCoordinates;
        float maxDistance = 0;

        for (int i = 0; i < heroCoordinates.Count; i++)
        {
            if (aliveStatus[i])
            {
                float distance = Vector2.Distance(heroCoordinates[i], bossManager.CurrentCoordinates);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestHero = heroCoordinates[i];
                }
            }
        }

        return farthestHero;
    }

    private void CastSacrificialBlood()
    {
        int bloodParticleCount = act > 1 ? 4 : 3;

        BossCombatObject bossCombatObject = new BossCombatObject(bossManager);
        CombatMethods.ApplayDamage(damage, bossCombatObject, bossCombatObject);

        MonoInstance.instance.StartCoroutine(SpawnAndCircleBloodParticles(bloodParticleCount));
    }

    private IEnumerator SpawnAndCircleBloodParticles(int count)
    {
        List<GameObject> bloodParticles = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject particle = MonoInstance.Instantiate(bloodParticlePrefab, bossManager.CurrentCoordinates, Quaternion.identity);
            bloodParticles.Add(particle);
        }

        float elapsedTime = 0f;

        while (elapsedTime < startCirclingTime)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < bloodParticles.Count; i++)
            {
                float angle = i * Mathf.PI * 2f / count + elapsedTime * Mathf.PI * 2f / startCirclingTime;

                float radius = Mathf.Lerp(0, circleBloodRadius, elapsedTime / startCirclingTime);

                Vector2 newPosition = bossManager.CurrentCoordinates + mapClass.tileZero + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius + centerModification;
                bloodParticles[i].transform.position = newPosition;
            }

            yield return null;
        }

        float normalCirclingTime = circlingTime - startCirclingTime - betweenSendTime;
        elapsedTime = 0f;

        while (elapsedTime < normalCirclingTime)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < bloodParticles.Count; i++)
            {
                float angle = i * Mathf.PI * 2f / count + elapsedTime * Mathf.PI * 2f / normalCirclingTime;
                Vector2 newPosition = bossManager.CurrentCoordinates + mapClass.tileZero + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleBloodRadius + centerModification;
                bloodParticles[i].transform.position = newPosition;
            }

            yield return null;
        }

        elapsedTime = 0f;
        float remainingTime = betweenSendTime * (count + 1) + 0.1f;
        int remainingCount = count - 1;
        while (elapsedTime < remainingTime)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > betweenSendTime * (bloodParticles.Count - remainingCount))
            {
                if (remainingCount == 0)
                {
                    yield return MonoInstance.instance.StartCoroutine(SendParticleToHeroPoint(bloodParticles[remainingCount]));

                    CastEnd();
                    break;
                }
                else 
                { 
                    remainingCount--;
                    MonoInstance.instance.StartCoroutine(SendParticleToHeroPoint(bloodParticles[remainingCount + 1]));
                }
            }

            for (int i = 0; i <= remainingCount; i++)
            {
                float angle = i * Mathf.PI * 2f / count + elapsedTime * Mathf.PI * 2f / normalCirclingTime;
                Vector2 newPosition = bossManager.CurrentCoordinates + mapClass.tileZero + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleBloodRadius + centerModification;
                if (bloodParticles[i] != null) bloodParticles[i].transform.position = newPosition;
            }

            yield return null;
        }
    }

    private IEnumerator SendParticleToHeroPoint(GameObject particle)
    {
        Vector2 startPosition = particle.transform.position;
        Vector2 targetPosition = GetFarthestHero() + mapClass.tileZero;

        float elapsedTime = 0f;

        while (elapsedTime < particleMoveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / particleMoveTime;

            if (particle != null) particle.transform.position = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        if (particle != null) particle.transform.position = targetPosition;
        MonoInstance.Destroy(particle);

        List<CombatObject> affectedCombatObjectList = GetAffectedCombatObjectList(new List<Vector2> { targetPosition - mapClass.tileZero });
        BossCombatObject bossCombatObject = new BossCombatObject(bossManager);
        if (affectedCombatObjectList.Count > 0)
        {

            foreach (CombatObject combatObject in affectedCombatObjectList)
            {
                if (combatObject is HeroCombatObject)
                {
                    float heal;
                    if (combatObject.GetData().CurrentShield > damage) heal = 0;
                    else if (combatObject.GetData().CurrentShield > 0) heal = damage - combatObject.GetData().CurrentShield;
                    else heal = damage;

                    CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject);
                    CombatMethods.ApplayHeal(heal, bossCombatObject, bossCombatObject);
                }
            }
        }
    }
}
