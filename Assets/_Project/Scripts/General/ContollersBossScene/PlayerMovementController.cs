using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectedPath
{
    public Vector2 SelectedTile;
    public List<Vector2> PathList;
}

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float timeBetweenMovement = 1f;
    
    public Vector2 LastPosition;
    public List<Vector2> MovementList = new();

    private List<Vector2> MovementListCheckPoints = new();
    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().gameplayTilemap;
    private Vector2 tileZero => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().tileZero;
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private bool isAlive => combatPlayerDataInStage.aliveStatus[playerInfoData.PlayerIDThisPlayer];
    private InputActions inputActions;
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerSkillManager playerSkillManager => FindObjectOfType<PlayerSkillManager>();
    private List<int> turnPriority => FindObjectOfType<PlayerInfoData>().TurnPriority;
    private int playerId => FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;

    public enum Movement
    {
        None = 0,
        HorVer = 2,
        Diagonal = 3,
        TooFar = 10
    }
    private Movement MovementIndex;

    private void OnEnable()
    {
        MovementList = new();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        inputActions = new InputActions();
        inputActions.Combat.SelectTile.performed += _ => AddNewPointsToList();
        inputActions.Combat.CancelAction.performed += _ => RemoveLastPointsInList();

        GlobalEventSystem.PlayerTurnEndConfirmed.AddListener(ApproveThePath);
        GlobalEventSystem.StartResultStageForPlayer.AddListener(StartMoving);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(StartDefineMovement);
        GlobalEventSystem.PlayerSkillChoosed.AddListener(PlayerSkillChoosed);
        GlobalEventSystem.PlayerSkillUnchoosed.AddListener(PlayerSkillUnchoosed);
    }

    private List<PathNode> CreatePathRoute(Vector2 fromTile, Vector2 toTile)
    {
        List<PathNode> paths = mapClass.gridPathfinding.FindPath((int)fromTile.x, (int)fromTile.y, (int)toTile.x, (int)toTile.y);
        return paths;
    }

    private void PlayerSkillUnchoosed(int skillNumber)
    {
        if (playerSkillManager.IsSkillListEmpty && isAlive) inputActions.Enable();
    }

    private void PlayerSkillChoosed(int skillNumber)
    {
        inputActions.Disable();
    }
    private void ChangeEnergy(int newEnergy)
    {
        ChangePlayerEnergyRpc(newEnergy, playerId);
        GlobalEventSystem.SendEnergyChange();
    }

    void StartDefineMovement()
    {
        if (isAlive)
        {
            inputActions.Enable();
            LastPosition = combatPlayerDataInStage.HeroCoordinates[playerId];
            MovementListCheckPoints.Add(LastPosition);
        }
    }

    public void RemoveLastPointsInList()
    {
        if (MovementList.Count > 0)
        {
            int lastCheckPointIndex = MovementList.LastIndexOf(MovementListCheckPoints[MovementListCheckPoints.Count - 2]);

            for (int i = lastCheckPointIndex; i < MovementList.Count; i++)
            {
                if (i == 0) DefineMovement(MovementList[i], MovementList[i]);
                else DefineMovement(MovementList[i], MovementList[i - 1]);

                int newEnergy = combatPlayerDataInStage._TotalStatsList[playerId].currentCombat.CurrentEnergy + (int)MovementIndex;
                ChangeEnergy(newEnergy);
            }

            MovementList.RemoveRange(lastCheckPointIndex, MovementList.Count - lastCheckPointIndex);

            if (MovementListCheckPoints.Count > 1) MovementListCheckPoints.RemoveAt(MovementListCheckPoints.Count - 1);

            if (MovementList.Count > 0) LastPosition = MovementList[MovementList.Count - 1];
            else LastPosition = combatPlayerDataInStage.HeroCoordinates[playerId];

            GlobalEventSystem.SendPathChanged();
        }
    }

    public void AddNewPointsToList()
    {
        Vector2 mouseWorldPos = inputActions.Combat.MousePosition.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        Vector3Int tile = gameplayTilemap.WorldToCell(mouseWorldPos);

        if (gameplayTilemap.HasTile(tile))
        {
            Vector2 tileCenterPos = gameplayTilemap.GetCellCenterWorld(tile);
            Vector2 targetPoint = tileCenterPos - tileZero;

            // ���������, ��� ������ ��������
            bool isFree = mapClass.IsPlayable(targetPoint);

            // ������� ���� �� ������� ������� �� ���������
            List<PathNode> pathNodes = CreatePathRoute(LastPosition, targetPoint);
            if (pathNodes[0] == LastPosition && combatPlayerDataInStage._TotalStatsList[playerId].currentCombat.CurrentEnergy == 0)
            {
                return; 
            }

            if (pathNodes != null && isFree && LastPosition != pathNodes[pathNodes.Count - 1])
            {
                for (int i = 0; i < pathNodes.Count; i++)
                {
                    Vector2 point = new Vector2(pathNodes[i].x, pathNodes[i].y);

                    DefineMovement(LastPosition, point);

                    if (combatPlayerDataInStage._TotalStatsList[playerId].currentCombat.CurrentEnergy >= (int)MovementIndex)
                    {
                        MovementList.Add(point);
                        LastPosition = point;

                        int newEnergy = combatPlayerDataInStage._TotalStatsList[playerId].currentCombat.CurrentEnergy - (int)MovementIndex;
                        ChangeEnergy(newEnergy);
                    }
                    else break;
                }

                GlobalEventSystem.SendPathChanged();
                MovementListCheckPoints.Add(LastPosition);
            }
        }
    }

    private void DefineMovement(Vector2 PlayerCoor, Vector2 targetPoint)
    {
        if (PlayerCoor.x == targetPoint.x && PlayerCoor.y == targetPoint.y) MovementIndex = Movement.None;
        else if (Math.Abs(PlayerCoor.x - targetPoint.x) > 1 || Math.Abs(PlayerCoor.y - targetPoint.y) > 1) MovementIndex = Movement.TooFar;
        else if (PlayerCoor.x == targetPoint.x || PlayerCoor.y == targetPoint.y) MovementIndex = Movement.HorVer;
        else MovementIndex = Movement.Diagonal;
    }

    private void ApproveThePath()
    {
        inputActions.Disable();
    }

    public List<Vector2> GetPath()
    {
        return MovementList;
    }

    private void StartMoving(int orderInTurnPriority)
    {
        if (turnPriority[orderInTurnPriority] == playerId) 
        {
            CorrectingPath();
            StartCoroutine("MovePlayer", orderInTurnPriority); 
        }
    }

    private void CorrectingPath()
    {
        for (int i = MovementList.Count - 1; i >= 0; i--)
        {
            List<MapObject> GetMapObjectLists = mapClass.GetMapObjectList(MovementList[i]);
            for (int j = 0; j < GetMapObjectLists.Count; j++)
            {
                Debug.Log(GetMapObjectLists[j]);
            }
            if (mapClass.GetMapObjectList(MovementList[i]).Exists(x => x is Hero)) MovementList.RemoveAt(i);
            else if (mapClass.GetMapObjectList(MovementList[i]).Exists(x => x is Boss)) MovementList.RemoveAt(i);
            else break;
        }
    }

    IEnumerator MovePlayer(int orderInTurnPriority)
    {
        if (MovementList.Count > 0)
        {
            for (int i = 0; i < MovementList.Count; i++)
            {
                if (combatPlayerDataInStage.aliveStatus[playerId])
                {
                    SendPlayerStartMoveRpc();
                    combatPlayerDataInStage.PlayersHeroes[playerId].transform.position = MovementList[i] + tileZero;
                    ChangePlayerCoordinatesRpc(MovementList[i], playerId);
                    SendPlayerEndMoveRpc();
                    ChangeMapStatesRpc(combatPlayerDataInStage.HeroCoordinates[playerId], MovementList[MovementList.Count - 1], playerId);
                    if (i < MovementList.Count - 1) yield return new WaitForSeconds(timeBetweenMovement);
                }
            }
            
            MovementList.Clear();
            GlobalEventSystem.SendPathChanged();
        }

        GlobalEventSystem.SendPlayerEndMoving(orderInTurnPriority);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendPlayerStartMoveRpc()
    {
        GlobalEventSystem.SendPlayerStartMove();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendPlayerEndMoveRpc()
    {
        GlobalEventSystem.SendPlayerEndMove();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangePlayerCoordinatesRpc(Vector2 newCoordinates, int id)
    {
        combatPlayerDataInStage.HeroCoordinates[id] = newCoordinates;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangePlayerEnergyRpc(int newEnergy, int id)
    {
        combatPlayerDataInStage._TotalStatsList[id].currentCombat.CurrentEnergy = newEnergy;
    }

    [Rpc(SendTo.ClientsAndHost)]
    void ChangeMapStatesRpc(Vector2 OldPlayerCoordinates, Vector2 NewPlayerCoordinates, int playerID)
    {
        mapClass.RemoveHero(OldPlayerCoordinates, playerID);
        mapClass.SetHero(NewPlayerCoordinates, playerID);
    }
}
