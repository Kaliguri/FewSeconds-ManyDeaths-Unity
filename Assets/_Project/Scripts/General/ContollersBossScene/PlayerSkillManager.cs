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
    private List<int> turnPriority => FindObjectOfType<PlayerInfoData>().TurnPriority;
    private PlayerMovementController playerMovementController => FindObjectOfType<PlayerMovementController>();
    MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => mapClass.gameplayTilemap;
    private Vector2 tileZero => mapClass.tileZero;
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private HeroData heroData => playerInfoData.HeroDataList[playerID];
    private Vector2 actualCastPosition => playerMovementController.LastPosition;
    private int _orderInTurnPriority;

    public void GetSkill(SkillScript skillScript, int SkillNumber)
    {
        ChoosenSkill = skillScript;
        skillID = SkillNumber;
        if (skillSelected) CanselAction();
        TargetPoints = 0;
        GlobalEventSystem.SendPlayerActionChoosed();
        skillSelected = true;
    }

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

    private void SelectSkillByButton(int skillNumber)
    {
        SkillScript skillByButton = heroData.SkillList[skillNumber].SkillVariationsList[playerInfoData.SkillChoiceList[playerID].variationList[skillNumber]].SkillScript;
        Debug.Log(skillNumber + " ChoosenSkill used");
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
            CastActionRpc(playerID, _skillID, CharacterCastCoordinate, actualCastPosition, TargetPoints, currentSkillIndex);

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
        if (TargetPoints < ChoosenSkill.TargetCount) CanselAction();
        else
        {
            skillSelected = false;
        }
    }

    void StartDefineSkills()
    {
        inputActions.Enable();
    }

    private void CanselAction()
    {
        skillSelected = false;
        GlobalEventSystem.SendPlayerActionUnchoosed();

        if (SkillList.Count > 0)
        {
            SkillList.RemoveAt(SkillList.Count - 1);
            skillNumberList.RemoveAt(skillNumberList.Count - 1);
            TargetTileList.RemoveAt(TargetTileList.Count - 1);
        }
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
                    //Debug.Log("AddFirstPoint");
                }
                else
                {
                    TargetTileList[TargetTileList.Count - 1].Add(selectedCellCoordinate);
                    //Debug.Log("AddNewPoint");
                }
                TargetPoints++;
            }

            if (TargetPoints == ChoosenSkill.TargetCount)
            { 
                skillSelected = false; 
                GlobalEventSystem.SendPlayerActionAproved();
            }
            else GlobalEventSystem.SendPlayerActionUpdate();
        }
    }
}
