using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [FoldoutGroup("General")]
    public BossVariationData Data;

    [FoldoutGroup("General")]
    public Vector2 SpawnCoordinates;

    [FoldoutGroup("General")]
    [ReadOnly]
    public GameObject BossGameObject;
    
    [FoldoutGroup("HP")]
    [ShowInInspector]
    public List<float> HPActList => Data.HPActList;

    [FoldoutGroup("HP")]
    [ReadOnly]
    public int CurrentAct = 0;

    [FoldoutGroup("HP")]
    [ReadOnly]
    public float MaxHPInCurrentAct => HPActList[CurrentAct];
    [FoldoutGroup("HP")]
    [ReadOnly]
    public float CurrentHPInCurrentAct;
    

    [FoldoutGroup("Combo")]
    [ReadOnly]
    public int CurrentAction = 0;

    [FoldoutGroup("Combo")]
    [ReadOnly]
    public BossComboData CurrentCombo;

    

    void Awake()
    {
        GlobalEventSystem.BossActionEnd.AddListener(CastAction);
        GlobalEventSystem.BossHPChanged.AddListener(CheckHP);
    }
    void Start()
    {
        Inizialize();
    }

    void Inizialize()
    {
        Spawn();
        HPInizialize();

        GlobalEventSystem.SendBossManagerInitialized();
        
    }

    void HPInizialize()
    {

        CurrentHPInCurrentAct = MaxHPInCurrentAct;
    }

    void CheckHP()
    {
        if (CurrentHPInCurrentAct <= 0)
        {
            ChangeAct();
        }
    }

    void ChangeAct()
    {
        CurrentAct += 1;
        HPInizialize();
        GlobalEventSystem.SendBossActChanged();

    }
    public void Spawn()
    {
        //Need ZeroPoint
        //BossGameObject = Instantiate(Data.GameObjectSpritePrefab); 
    }

    public void CastCombo()
    {
        List<BossComboData> ComboList = Data.AttacksInActList[CurrentAct].ComboAttackList;
        CurrentCombo = ComboList[Random.Range(0, ComboList.Count)];

        CurrentAction = 0;

        CastAction();

    }

    private void CastAction()
    {
        if (CurrentAction < CurrentCombo.BossActionList.Count)
        {
        CurrentCombo.BossActionList[CurrentAction].ActionScript.Cast(CurrentAct);
        CurrentAction ++;
        }
        else
        {
            Debug.Log("Event Combo End");
        }
    }
}
