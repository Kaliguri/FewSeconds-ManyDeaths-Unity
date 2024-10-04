using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BossManager : NetworkBehaviour
{
    [FoldoutGroup("General")]
    public BossVariationData Data;

    [FoldoutGroup("General")]
    [SerializeField] private Vector2 SpawnCoordinates;
    [HideInInspector]
    public Vector2 CurrentCoordinates;

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

    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Vector2 tileZero => mapClass.tileZero;
    private List<List<Vector2>> TargetPointsForActions = new();
    private float TimeBetweenActions = 1f;

    void Awake()
    {
        GlobalEventSystem.BossActionEnd.AddListener(CastActionAfterTime);
        GlobalEventSystem.BossHPChanged.AddListener(CheckHP);
    }

    private void CastActionAfterTime()
    {
        Invoke(nameof(CastAction), TimeBetweenActions);
    }

    void Start()
    {
        Inizialize();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
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
        BossGameObject = Instantiate(Data.GameObjectSpritePrefab, SpawnCoordinates + tileZero, Quaternion.identity);
        mapClass.SetBoss(SpawnCoordinates);
        CurrentCoordinates = SpawnCoordinates;
    }

    public void ChoiceCombo()
    {
        List<BossComboData> ComboList = Data.AttacksInActList[CurrentAct].ComboAttackList;
        int currentComboIndex = Random.Range(0, ComboList.Count);
        ChoiceComboRpc(currentComboIndex);
        GetTargetPointsForActions();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChoiceComboRpc(int comboIndex)
    {
        List<BossComboData> ComboList = Data.AttacksInActList[CurrentAct].ComboAttackList;
        CurrentCombo = ComboList[comboIndex];
    }

    private void GetTargetPointsForActions()
    {
        for (int i = 0; i < CurrentCombo.BossActionList.Count; i++)
        {
            List<Vector2> TargetPoints = CurrentCombo.BossActionList[i].ActionScript.GetCastPoint(CurrentAct);
            GetTargetPointsForActionsRpc(TargetPoints.ToArray());
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void GetTargetPointsForActionsRpc(Vector2[] TargetPoints)
    {
        TargetPointsForActions.Add(TargetPoints.ToList());
    }

    public void CastCombo()
    {
        CurrentAction = 0;

        CastAction();
    }

    private void CastAction()
    {
        if (CurrentAction < CurrentCombo.BossActionList.Count)
        {
            CurrentCombo.BossActionList[CurrentAction].ActionScript.Cast(TargetPointsForActions[CurrentAction], CurrentAct);
            CurrentAction++;
        }
        else
        {
            ClearTargetPointsForActions();
            GlobalEventSystem.SendBossEndCombo();
        }
    }

    private void ClearTargetPointsForActions()
    {
        TargetPointsForActions.Clear();
    }
}
