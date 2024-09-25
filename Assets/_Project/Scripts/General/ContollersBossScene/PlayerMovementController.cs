using System;
using System.Collections;
using System.Collections.Generic;
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
    public Vector2 LastPosition;
    public List<Vector2> MovementList = new();

    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().gameplayTilemap;
    private Vector2 tileZero => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().tileZero;
    private InputActions inputActions;
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerSkillManager playerSkillManager => FindObjectOfType<PlayerSkillManager>();
    private List<int> turnPriority => FindObjectOfType<PlayerInfoData>().TurnPriority;
    private int localId => (int)NetworkManager.Singleton.LocalClientId;
    private float timeBetweenMovement = 1f;

    enum Movement
    {
        HorVer = 2,
        Diagonal = 3,
        TooFar = 10
    }
    private Movement MovementIndex;

    private void OnEnable()
    {
        MovementList = new();
    }

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        //Debug.Log("awakeByOwnerNet");
        inputActions = new InputActions();
        inputActions.Combat.SelectTile.performed += _ => AddNewPointToList();
        inputActions.Combat.CancelAction.performed += _ => RemoveLastPointInList();

        GlobalEventSystem.PlayerTurnEndConfirmed.AddListener(ApproveThePath);
        GlobalEventSystem.ResultStageStarted.AddListener(StartMoving);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(StartDefineMovement);
        GlobalEventSystem.PlayerActionChoosed.AddListener(PlayerActionChoosed);
        GlobalEventSystem.PlayerActionUnchoosed.AddListener(PlayerActionUnchoosed);
    }

    private void PlayerActionUnchoosed()
    {
        if (playerSkillManager.IsSkillListEmpty) inputActions.Enable();
    }

    private void PlayerActionChoosed()
    {
        inputActions.Disable();
    }

    void StartDefineMovement()
    {
        inputActions.Enable();
        LastPosition = combatPlayerDataInStage.HeroCoordinates[localId];
    }

    public void RemoveLastPointInList()
    {
        if (MovementList.Count > 0)
        {
            //Debug.Log("Remove");
            //Debug.Log(MovementList[MovementList.Count - 1]);

            if (MovementList.Count == 1) 
            { 
                DefineMovement(combatPlayerDataInStage.HeroCoordinates[localId], MovementList[0]);
                LastPosition = combatPlayerDataInStage.HeroCoordinates[localId];
            }
            else 
            { 
                DefineMovement(MovementList[MovementList.Count - 1], MovementList[MovementList.Count - 2]);
                LastPosition = MovementList[MovementList.Count - 2];
            }

            int newEnergy = combatPlayerDataInStage._TotalStatsList[localId].currentCombat.CurrentEnergy + (int)MovementIndex;
            ChangeEnergy(newEnergy);

            mapClass.ChangeCell(MovementList[MovementList.Count - 1], MapClass.TileStates.Free);
            MovementList.RemoveAt(MovementList.Count - 1);

            GlobalEventSystem.SendPathChanged();
        }
    }

    private void ChangeEnergy(int newEnergy)
    {
        int playerId = (int)combatPlayerDataInStage.PlayersHeroes[localId].GetComponent<NetworkObject>().OwnerClientId;
        ChangePlayerEnergyRpc(newEnergy, playerId);
        GlobalEventSystem.SendEnergyChange();
    }

    public void AddNewPointToList()
    {
        Vector2 mouseWorldPos = inputActions.Movement.MousePosition.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        Vector3Int tile = gameplayTilemap.WorldToCell(mouseWorldPos);

        if (gameplayTilemap.HasTile(tile))
        {

            Vector2 tileCenterPos = gameplayTilemap.GetCellCenterWorld(tile);
            Vector2 targetPoint = tileCenterPos - tileZero;

            bool isFree = mapClass.IsFree(targetPoint);

            DefineMovement(LastPosition, targetPoint);

            if (isFree && MovementIndex != Movement.TooFar && combatPlayerDataInStage._TotalStatsList[localId].currentCombat.CurrentEnergy >= (int)MovementIndex)
            {
                MovementList.Add(targetPoint);
                LastPosition = targetPoint;
                GlobalEventSystem.SendPathChanged();

                mapClass.ChangeCell(targetPoint, MapClass.TileStates.Player);

                int newEnergy = combatPlayerDataInStage._TotalStatsList[localId].currentCombat.CurrentEnergy - (int)MovementIndex;
                ChangeEnergy(newEnergy);
            }
        }
    }

    private void DefineMovement(Vector2 PlayerCoor, Vector2 targetPoint)
    {
        if (Math.Abs(PlayerCoor.x - targetPoint.x) > 1 || Math.Abs(PlayerCoor.y - targetPoint.y) > 1) MovementIndex = Movement.TooFar;
        else if (PlayerCoor.x == targetPoint.x || PlayerCoor.y == targetPoint.y) MovementIndex = Movement.HorVer;
        else MovementIndex = Movement.Diagonal;
    }

    private void OnDisable()
    {
        inputActions.Disable();
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
        if (turnPriority[orderInTurnPriority] == localId) StartCoroutine("MovePlayer", orderInTurnPriority);
    }

    IEnumerator MovePlayer(int orderInTurnPriority)
    {
        if (MovementList.Count > 0)
        {
            for (int i = 0; i < MovementList.Count; i++)
            {
                SendPlayerStartMoveRpc();
                combatPlayerDataInStage.PlayersHeroes[localId].transform.position = MovementList[i] + tileZero;
                ChangeMapStatesRpc(combatPlayerDataInStage.HeroCoordinates[localId], MovementList[MovementList.Count - 1], MapClass.TileStates.Player);
                ChangePlayerCoordinatesRpc(MovementList[i], (int)combatPlayerDataInStage.PlayersHeroes[localId].GetComponent<NetworkObject>().OwnerClientId);
                SendPlayerEndMoveRpc();
                yield return new WaitForSeconds(timeBetweenMovement);
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
    void ChangeMapStatesRpc(Vector2 OldPlayerCoordinates, Vector2 NewPlayerCoordinates, MapClass.TileStates State)
    {
        mapClass.ChangeCell(OldPlayerCoordinates, MapClass.TileStates.Free);
        mapClass.ChangeCell(NewPlayerCoordinates, State);
    }
}
