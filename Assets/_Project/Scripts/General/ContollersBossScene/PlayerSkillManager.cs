using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;


public class PlayerSkillManager : NetworkBehaviour
{

    [SerializeField] private float timeBetweenPlayerCast = 1f;

    public bool IsSkillListEmpty => (0 == SkillList.Count);
    public SkillScript ChoosenSkill;
    public int TargetPoints = 0;
    public List<List<Vector2>> TargetTileList = new();
    public List<SkillScript> SkillList = new();

    private int skillID;
    private int currentAction;
    private int skillIndex;
    private bool skillSelected;
    private InputActions inputActions;
    private List<int> skillNumberList = new();
    private List<Vector2> characterCastCoordinate = new();
    private List<Vector2> availableTilesList = new();
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private SkillCooldownManager skillCooldownManager => GameObject.FindObjectOfType<SkillCooldownManager>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private List<int> turnPriority => FindObjectOfType<PlayerInfoData>().TurnPriority;
    private PlayerMovementController playerMovementController => FindObjectOfType<PlayerMovementController>();
    MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => mapClass.gameplayTilemap;
    private Vector2 tileZero => mapClass.tileZero;
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private HeroData heroData => playerInfoData.HeroDataList[playerID];
    private Vector2 actualCastPosition => playerMovementController.LastPosition;
    private int _orderInTurnPriority;


    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Combat.SelectTile.performed += _ => AddSkillToList();
        inputActions.Combat.CancelAction.performed += _ => CanselAction();
        inputActions.Combat.SkillQ.performed += _ => SelectSkillByButton(0);
        inputActions.Combat.SkillW.performed += _ => SelectSkillByButton(1);
        inputActions.Combat.SkillE.performed += _ => SelectSkillByButton(2);
        inputActions.Combat.SkillR.performed += _ => SelectSkillByButton(3);

        GlobalEventSystem.PlayerTurnStageStarted.AddListener(StartDefineSkills);
        GlobalEventSystem.PlayerTurnEndConfirmed.AddListener(ApproveTheSkills);
        GlobalEventSystem.StartCastPlayer.AddListener(StartSkillSystem);
        GlobalEventSystem.PlayerActionEnd.AddListener(CastAction);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void GetSkill(SkillScript skillScript, int SkillNumber)
    {
        if (skillCooldownManager.GetSkillCooldown(playerID, SkillNumber) == 0 && combatPlayerDataInStage._TotalStatsList[playerID].currentCombat.CurrentEnergy >= skillScript.EnergyCost)
        {
            ChoosenSkill = skillScript;
            skillID = SkillNumber;
            if (skillSelected) CanselAction();
            TargetPoints = 0;
            int newEnergy = combatPlayerDataInStage._TotalStatsList[playerID].currentCombat.CurrentEnergy - ChoosenSkill.EnergyCost;
            ChangeEnergy(newEnergy);
            GlobalEventSystem.SendPlayerActionChoosed();
            skillSelected = true;
        }
    }

    private void SelectSkillByButton(int skillNumber)
    {
        SkillScript skillByButton = heroData.SkillList[skillNumber].SkillVariationsList[playerInfoData.SkillChoiceList[playerID].variationList[skillNumber]].SkillScript;
        GetSkill(skillByButton, skillNumber);
    }

    private void StartSkillSystem(int orderInTurnPriority)
    {
        if (turnPriority[orderInTurnPriority] == playerID)
        {
            _orderInTurnPriority = orderInTurnPriority;
            currentAction = 0;
            skillIndex = 0;
            CastAction();
        }
    }

    private void CastAction()
    {
        if (currentAction < SkillList.Count)
        {
            Vector2[] TargetPoints = TargetTileList[currentAction].ToArray();
            Vector2 CharacterCastCoordinate = characterCastCoordinate[currentAction];
            int _skillID = skillNumberList[currentAction];
            int currentSkillIndex = skillIndex;
            CastActionRpc(playerID, _skillID, CharacterCastCoordinate, combatPlayerDataInStage.HeroCoordinates[playerID], TargetPoints, currentSkillIndex);

            skillIndex++;
            if (skillIndex == SkillList[currentAction].TargetCount)
            {
                currentAction++;
                skillIndex = 0;
            }
        }
        else
        {
            SkillList.Clear();
            skillNumberList.Clear();
            TargetTileList.Clear();
            Invoke("SendPlayerEndResultTurnEvent", timeBetweenPlayerCast);
        }
    }

    private void SendPlayerEndResultTurnEvent()
    {
        GlobalEventSystem.SendPlayerEndResultTurn(_orderInTurnPriority);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void CastActionRpc(int casterPlayerId, int skillNumber, Vector2 CasterPosition, Vector2 ActualCasterPosition, Vector2[] TargetPoints, int skillIndex)
    {
        int variation = playerInfoData.SkillChoiceList[casterPlayerId].variationList[skillNumber];
        SkillScript skillScript = heroData.SkillList[skillNumber].SkillVariationsList[variation].SkillScript;
        skillScript.Cast(CasterPosition, ActualCasterPosition, TargetPoints, casterPlayerId, skillIndex);
    }

    private void ApproveTheSkills()
    {
        inputActions.Disable();
        if (skillSelected) CanselAction();
    }

    void StartDefineSkills()
    {
        inputActions.Enable();
    }

    private void CanselAction()
    {
        if (ChoosenSkill != null)
        {
            SendChangeSkillCooldownRpc(playerID, skillID, 0);
            int newEnergy = combatPlayerDataInStage._TotalStatsList[playerID].currentCombat.CurrentEnergy + ChoosenSkill.EnergyCost;
            ChangeEnergy(newEnergy);
            ChoosenSkill = null;
        }

        skillSelected = false;

        if (SkillList.Count > 0)
        {
            SkillList.RemoveAt(SkillList.Count - 1);
            skillNumberList.RemoveAt(skillNumberList.Count - 1);
            TargetTileList.RemoveAt(TargetTileList.Count - 1);
        }

        GlobalEventSystem.SendPlayerActionUnchoosed();
    }

    private void AddSkillToList()
    {
        if (skillSelected)
        {
            Vector2 mouseWorldPos = inputActions.Combat.MousePosition.ReadValue<Vector2>();
            mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
            Vector3Int tile = gameplayTilemap.WorldToCell(mouseWorldPos);
            Vector2 tileCenterPos = gameplayTilemap.GetCellCenterWorld(tile);
            Vector2 selectedCellCoordinate = tileCenterPos - tileZero;

            if (TargetPoints < ChoosenSkill.TargetCount)
            {

                availableTilesList = ChoosenSkill.AvailableTiles(actualCastPosition, selectedCellCoordinate, TargetPoints);

                if (!availableTilesList.Contains(selectedCellCoordinate)) return;


                if (TargetPoints == 0) 
                {
                    SkillList.Add(ChoosenSkill);
                    skillNumberList.Add(skillID);
                    characterCastCoordinate.Add(actualCastPosition);
                    TargetTileList.Add(new List<Vector2> { selectedCellCoordinate });
                }
                else
                {
                    TargetTileList[TargetTileList.Count - 1].Add(selectedCellCoordinate);
                }
                TargetPoints++;
            }

            if (TargetPoints == ChoosenSkill.TargetCount)
            { 
                skillSelected = false;
                GlobalEventSystem.SendPlayerActionAproved();
                SendChangeSkillCooldownRpc(playerID, skillID, ChoosenSkill.SkillCooldown);
            }
            else GlobalEventSystem.SendPlayerActionUpdate();
        }
    }

    private void ChangeEnergy(int newEnergy)
    {
        ChangePlayerEnergyRpc(newEnergy, playerID);
        GlobalEventSystem.SendEnergyChange();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangePlayerEnergyRpc(int newEnergy, int id)
    {
        combatPlayerDataInStage._TotalStatsList[id].currentCombat.CurrentEnergy = newEnergy;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendChangeSkillCooldownRpc(int playerId, int skillId, int skillCooldown)
    {
        skillCooldownManager.SetSkillCooldown(playerId, skillId, skillCooldown);
    }
}