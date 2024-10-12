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
    public GameObject GhostBossGameObject;

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
    public float CurrentHPInCurrentAct => bossStats.CurrentHP;
    

    [FoldoutGroup("Combo")]
    [ReadOnly]
    public int CurrentAction = 0;

    [FoldoutGroup("Combo")]
    [ReadOnly]
    public BossComboData CurrentCombo;

    [HideInInspector]
    public CombatStats bossStats;

    [Header("Stats")]
    public float alfhaForGhost = 0.5f;

    private CombatPlayerDataInStage combatPlayerDataInStage => GameObject.FindObjectOfType<CombatPlayerDataInStage>();
    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Vector2 tileZero => mapClass.tileZero;
    private List<List<Vector2>> TargetPointsForActions = new();
    private float TimeBetweenActions = 1f;
    public static BossManager instance = null;


    void Awake()
    {
        GlobalEventSystem.BossActionEnd.AddListener(CastActionAfterTime);
        GlobalEventSystem.BossHPChanged.AddListener(CheckHP);
        GlobalEventSystem.PlayerTurnEnding.AddListener(StopCombo);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(EnableGhostBoss);
        if (instance == null) instance = this;
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
        bossStats.MaxHP = MaxHPInCurrentAct;
        bossStats.CurrentHP = MaxHPInCurrentAct;
    }

    private void CastActionAfterTime()
    {
        Invoke(nameof(CastAction), TimeBetweenActions);
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
        if (CurrentAct + 1 < HPActList.Count)
        {
            CurrentAct += 1;
            HPInizialize();
            GlobalEventSystem.SendBossActChanged();
        }
        else
        {
            GlobalEventSystem.SendBossDied();
        }
    }

    public void Spawn()
    {
        BossGameObject = Instantiate(Data.GameObjectSpritePrefab, SpawnCoordinates + tileZero, Quaternion.identity);
        GhostBossGameObject = Instantiate(Data.GameObjectSpritePrefab, SpawnCoordinates + tileZero, Quaternion.identity);
        Color bossColor = GhostBossGameObject.GetComponentInChildren<SpriteRenderer>().color;
        GhostBossGameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(bossColor.r, bossColor.g, bossColor.b, bossColor.a * alfhaForGhost);
        GhostBossGameObject.SetActive(false);
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
        SpawnCoordinates = CurrentCoordinates;
        CastAction();
    }

    private void CastAction()
    {
        Debug.Log(CurrentCombo);
        if (CurrentAction < CurrentCombo.BossActionList.Count)
        {
            CurrentCombo.BossActionList[CurrentAction].ActionScript.Cast(TargetPointsForActions[CurrentAction], CurrentAct);
            CurrentAction++;
        }
        else
        {
            if (CombatStageManager.instance.currentStage is BossTurnStage) ClearTargetPointsForActions();
            else
            {
                BossGameObject.transform.position = SpawnCoordinates + mapClass.tileZero;
                GhostBossGameObject.transform.position = SpawnCoordinates + mapClass.tileZero;
                CurrentCoordinates = SpawnCoordinates;
            }
            GlobalEventSystem.SendBossEndCombo();
        }
    }

    private void StopCombo()
    {
        CurrentAction = 3259720;
        GhostBossGameObject.SetActive(false);
    }

    private void EnableGhostBoss()
    {
        GhostBossGameObject.SetActive(true);
        GhostBossGameObject.transform.position = BossGameObject.transform.position;
    }

    private void ClearTargetPointsForActions()
    {
        TargetPointsForActions.Clear();
    }
}
