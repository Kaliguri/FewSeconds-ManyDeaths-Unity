using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;


public class alizer : NetworkBehaviour
{
    #region Tile Sprites

    [TabGroup("Prefabs")]
    [Title("Standart")]
    [SerializeField] GameObject standartTileSelector;

    [TabGroup("Prefabs")]
    [Title("For Person")]
    [SerializeField] GameObject heroTile;

    [TabGroup("Prefabs")]
    [Title("For Tile Type")]
    [SerializeField] GameObject allyTypeTileSelector;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject enemyTypeTileSelector;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject terrainTypeTileSelector;

    [TabGroup("Prefabs")]
    [Title("For Skill Select")]
    [SerializeField] GameObject skillSelectTileSelector;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject skillAffectedTilePrefab;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject skillAvaliableTilePrefab;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject skillChoosedAffectedTilePrefab;

    [TabGroup("Prefabs")]
    [Title("For Movement Path")]
    [SerializeField] GameObject pathTilePrefab;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject pathEndTilePrefab;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject pathTileSelectorPrefab;
    [TabGroup("Prefabs")]
    [SerializeField] GameObject pathEndTileSelectorPrefab;

    [Space]

    #endregion

    #region ChangableParameters
    [TabGroup("For GridAreaMethods Test")]
    [SerializeField] GameObject VisualizeSpritePrefab;

    [TabGroup("For GridAreaMethods Test")]
    [SerializeField] GridAreaMethods.figs fig;

    [TabGroup("For GridAreaMethods Test")]
    [SerializeField] Vector2 character, cell;

    [TabGroup("For GridAreaMethods Test")]
    [SerializeField] int width = 1, maxDistance = 5, cutValue = 0;
    [TabGroup("For GridAreaMethods Test")]
    public List<Vector2> VisualizeCoordinateList = new();
    #endregion

    #region LinkParameters
    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => mapClass.gameplayTilemap;
    private Vector2 tileZero => mapClass.tileZero;
    private PlayerSkillManager playerSkillManager => FindObjectOfType<PlayerSkillManager>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private SkillScript skillSelected => FindObjectOfType<PlayerSkillManager>().ChoosenSkill;
    private int targetPoints => FindObjectOfType<PlayerSkillManager>().TargetPoints;
    private List<List<Vector2>> TargetTileList => FindObjectOfType<PlayerSkillManager>().TargetTileList;
    private List<SkillScript> SkillList => FindObjectOfType<PlayerSkillManager>().SkillList;
    private int playerID => GameObject.FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;
    private PlayerMovementController playerMovementController => FindObjectOfType<PlayerMovementController>();
    Vector2[] heroPositions => combatPlayerDataInStage.HeroCoordinates;
    private List<Vector2> pathCoordinates => playerMovementController.MovementList;
    private Vector2 selectedCellCoordinate => playerMovementController.LastPosition;
    private bool isAlive => combatPlayerDataInStage.aliveStatus[playerInfoData.PlayerIDThisPlayer];
    #endregion

    #region PrivateParameters
    private List<GameObject> playerPathList = new();
    private List<GameObject> playersPathList = new();
    private List<GameObject> availableTileGameObjectList = new();
    private List<GameObject> playersTilesGameObjectList = new();
    private List<GameObject> playersAffectedTilesList = new();
    private GameObject tileSelector;
    private GameObject tileSkillSelector;
    private InputActions inputActions;
    private bool isPlayerCasting = false;
    private bool isPlayerMoving = false;
    private bool isPlayerTurnStage = false;
    #endregion

    void Awake()
    {
        inputActions = new InputActions();

        GlobalEventSystem.PlayerTurnStageStarted.AddListener(ShowPlayersTiles);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(PlayerTurnStageStarted);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(PlayersStartMoving);
        GlobalEventSystem.PlayerTurnStageEnded.AddListener(PlayersEndMoving);
        GlobalEventSystem.PlayerTurnEndConfirmed.AddListener(PlayersEndMoving);
        GlobalEventSystem.AllPlayerSpawned.AddListener(CreatePlayersTiles);
        GlobalEventSystem.PlayerDied.AddListener(HideDeadPlayerTile);
        GlobalEventSystem.PlayerStartMove.AddListener(HidePlayersTiles);
        GlobalEventSystem.PlayerEndMove.AddListener(ShowPlayersTiles);
        GlobalEventSystem.PlayerSkillChoosed.AddListener(PlayerSkillChoosed);
        GlobalEventSystem.PlayerSkillUpdate.AddListener(PlayerSkillUpdate);
        GlobalEventSystem.PlayerSkillUnchoosed.AddListener(PlayerSkillUnchoosed);
        GlobalEventSystem.PlayerSkillUnchoosed.AddListener(PlayerStartMoving);
        GlobalEventSystem.PlayerSkillAproved.AddListener(PlayerSkillUnchoosed);
        GlobalEventSystem.PathChanged.AddListener(ChangePath);
        GlobalEventSystem.ResultStageStarted.AddListener(SendClearAprovedAffectedAreas);
        GlobalEventSystem.ResultStageStarted.AddListener(SendPathTiles);
        GlobalEventSystem.ResultStageStarted.AddListener(PlayerTurnStageEnded);
        GlobalEventSystem.AllPlayersEndMoving.AddListener(ClearPlayersPathTiles);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    void Start()
    {
        inputActions.Enable();
    }

    private void FixedUpdate()
    {
        UpdateTileSelector();
    }

    private void UpdateTileSelector()
    {
        if (tileSelector != null)
        {
            Destroy(tileSelector);
            tileSelector = null;
        }

        if (tileSkillSelector != null)
        {
            Destroy(tileSkillSelector);
            tileSkillSelector = null;
        }

        Vector2 mouseWorldPos = inputActions.Combat.MousePosition.ReadValue<Vector2>();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        Vector3Int tile = gameplayTilemap.WorldToCell(mouseWorldPos);

        if (gameplayTilemap.HasTile(tile))
        {
            Vector2 TileCenter = gameplayTilemap.GetCellCenterWorld(tile);
            List<MapObject> GetMapObjectList = mapClass.GetMapObjectList(TileCenter - tileZero);

            if (isPlayerCasting && isAlive) UpdateSkillAffectedArea(TileCenter);
            else
            {
                if (GetMapObjectList.Exists(x => x is Hero)) tileSelector = Instantiate(allyTypeTileSelector, TileCenter, Quaternion.identity);
                else if (GetMapObjectList.Exists(x => x is Boss)) tileSelector = Instantiate(enemyTypeTileSelector, TileCenter, Quaternion.identity);
                else if (GetMapObjectList.Exists(x => x is TempBloked)) tileSelector = Instantiate(terrainTypeTileSelector, TileCenter, Quaternion.identity);
                else tileSelector = Instantiate(standartTileSelector, TileCenter, Quaternion.identity);

                if (isPlayerMoving && isPlayerTurnStage && isAlive) UpdatePath(TileCenter);
            }
        }
    }

    private void UpdatePath(Vector2 tileCenter)
    {
        Vector2 tileIndex = tileCenter - tileZero;
        List<PathNode> path = mapClass.gridPathfinding.FindPath((int)selectedCellCoordinate.x, (int)selectedCellCoordinate.y, (int)tileIndex.x, (int)tileIndex.y);
        Vector2 LastPosition = selectedCellCoordinate;
        int currentEnergy = combatPlayerDataInStage._TotalStatsList[playerID].currentCombat.CurrentEnergy;

        for (int i = 0; i < path.Count; i++)
        {
            Vector2 point = new Vector2(path[i].x, path[i].y);

            PlayerMovementController.Movement MovementIndex = DefineMovement(LastPosition, point);

            if (currentEnergy >= (int)MovementIndex)
            {
                GameObject pathTile;
                if (i == path.Count - 1) pathTile = Instantiate(pathEndTileSelectorPrefab, point + tileZero, Quaternion.identity);
                else pathTile = Instantiate(pathTileSelectorPrefab, point + tileZero, Quaternion.identity);
                currentEnergy -= (int)MovementIndex;
                LastPosition = point;
                Destroy(pathTile, Time.fixedDeltaTime);
            }
            else break;
        }
    }

    private PlayerMovementController.Movement DefineMovement(Vector2 PlayerCoor, Vector2 targetPoint)
    {
        PlayerMovementController.Movement MovementIndex;
        if (PlayerCoor.x == targetPoint.x && PlayerCoor.y == targetPoint.y) MovementIndex = PlayerMovementController.Movement.None;
        else if (Math.Abs(PlayerCoor.x - targetPoint.x) > 1 || Math.Abs(PlayerCoor.y - targetPoint.y) > 1) MovementIndex = PlayerMovementController.Movement.TooFar;
        else if (PlayerCoor.x == targetPoint.x || PlayerCoor.y == targetPoint.y) MovementIndex = PlayerMovementController.Movement.HorVer;
        else MovementIndex = PlayerMovementController.Movement.Diagonal;
        return MovementIndex;
    }

    private void PlayersEndMoving()
    {
        isPlayerMoving = false;
    }

    private void PlayerTurnStageEnded()
    {
        isPlayerTurnStage = false;
    }

    private void PlayerTurnStageStarted()
    {
        isPlayerTurnStage = true;
    }

    private void PlayersStartMoving()
    {
        if (playerSkillManager.IsSkillListEmpty) isPlayerMoving = true;
    }

    private void PlayerStartMoving(int skillNumber)
    {
        PlayersStartMoving();
    }

    #region PlayersPath

    private void SendPathTiles()
    {
        ClearPath();
        bool isLast = false;
        for (int i = 1; i < pathCoordinates.Count; i++)
        {
            if (i == pathCoordinates.Count - 1) isLast = true;
            SendPlayerPathTileRpc(pathCoordinates[i] + tileZero, isLast, playerID);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendPlayerPathTileRpc(Vector2 Location, bool isLast, int PlayerID)
    {
        GameObject pathObject;
        if (isLast) pathObject = Instantiate(pathEndTilePrefab, Location, Quaternion.identity);
        else pathObject = Instantiate(pathTilePrefab, Location, Quaternion.identity);
        Color color = playerInfoData.ColorList[PlayerID];
        pathObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, pathObject.GetComponent<SpriteRenderer>().color.a);
        playersPathList.Add(pathObject);
    }

    private void ClearPlayersPathTiles()
    {
        for (int i = 0; i < playersPathList.Count; i++)
        {
            GameObject pathObject = playersPathList[i];
            Destroy(pathObject);
        }
        playersPathList.Clear();
    }
    #endregion

    #region PlayerSkill
    private void PlayerSkillUpdate(int skillNumber)
    {
        UpdateAprovedAffectedAreas();
        UpdateSkillAvaliableArea();
    }

    private void PlayerSkillUnchoosed(int skillNumber)
    {
        UpdateAprovedAffectedAreas();
        isPlayerCasting = false;
        ClearAvaliableArea();
    }

    private void PlayerSkillChoosed(int skillNumber)
    {
        isPlayerCasting = true;
        isPlayerMoving = false;
        UpdateSkillAvaliableArea();
    }

    #endregion

    #region PlayersTiles

    private void CreatePlayersTiles()
    {
        for (int i = 0; i < heroPositions.Length; i++)
        {
            GameObject playerTile = Instantiate(heroTile, heroPositions[i] + mapClass.tileZero, Quaternion.identity);

            Color color = playerInfoData.ColorList[i];
            playerTile.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, playerTile.GetComponent<SpriteRenderer>().color.a);

            playersTilesGameObjectList.Add(playerTile);
        }
        HidePlayersTiles();
    }

    private void HidePlayersTiles()
    {
        for (int i = 0; i < playersTilesGameObjectList.Count; i++)
        {
            playersTilesGameObjectList[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void ShowPlayersTiles()
    {
        for (int i = 0; i < playersTilesGameObjectList.Count; i++)
        {
            playersTilesGameObjectList[i].transform.position = heroPositions[i] + tileZero;
            if (combatPlayerDataInStage.aliveStatus[i]) playersTilesGameObjectList[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void HideDeadPlayerTile(int playerID)
    {
        playersTilesGameObjectList[playerID].GetComponent<SpriteRenderer>().enabled = false;
    }
    #endregion

    #region AvaliableArea

    private void UpdateSkillAvaliableArea()
    {

        ClearAvaliableArea();
        List<Vector2> availableTilesList = skillSelected.AvailableTiles(selectedCellCoordinate, selectedCellCoordinate, targetPoints);
        for (int i = 0; i < availableTilesList.Count; i++)
        {
            Vector3Int tileAvaliable = gameplayTilemap.WorldToCell(availableTilesList[i] + tileZero);
            if (gameplayTilemap.HasTile(tileAvaliable))
            {
                GameObject availableTile = Instantiate(skillAvaliableTilePrefab, availableTilesList[i] + tileZero, Quaternion.identity);
                availableTileGameObjectList.Add(availableTile);
            }
        }
        
    }

    private void ClearAvaliableArea()
    {
        for (int i = 0; i < availableTileGameObjectList.Count; i++)
        {
            GameObject avaliableTileGameObject = availableTileGameObjectList[i];
            Destroy(avaliableTileGameObject);
        }
        availableTileGameObjectList.Clear();
    }

    #endregion

    #region AffectedArea
    private void SendClearAprovedAffectedAreas()
    {
        ClearAprovedAffectedAreas();
    }

    private void ClearAprovedAffectedAreas()
    {
        for (int i = 0; i < playersAffectedTilesList.Count; i++)
        {
            GameObject tile = playersAffectedTilesList[i];
            Destroy(tile);
        }
        playersAffectedTilesList.Clear();
    }

    private void UpdateSkillAffectedArea(Vector2 TileCenter)
    {
        List<Vector2> affectedAreaList = skillSelected.Area(selectedCellCoordinate, TileCenter - tileZero, targetPoints);
        for (int i = 0; i < affectedAreaList.Count; i++)
        {
            Vector3Int tile = gameplayTilemap.WorldToCell(affectedAreaList[i] + tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject areaTile = Instantiate(skillAffectedTilePrefab, affectedAreaList[i] + tileZero, Quaternion.identity);
                Destroy(areaTile, Time.fixedDeltaTime);
            }
        }
    }

    private void UpdateAprovedAffectedAreas()
    {
        ClearAprovedAffectedAreas();
        for (int i = 0; i < TargetTileList.Count; i++)
        {
            SkillScript skill = SkillList[i];
            for (int skillIndex = 0; skillIndex < TargetTileList[i].Count; skillIndex++)
            {
                CreateAprovedAffectedArea(skill, i, skillIndex);
            }
        }
    }

    private void CreateAprovedAffectedArea(SkillScript skill, int i, int skillIndex)
    {
        List<Vector2> affectedAreaList = skill.Area(selectedCellCoordinate, TargetTileList[i][skillIndex], skillIndex);
        for (int k = 0; k < affectedAreaList.Count; k++)
        {
            Vector3Int tile = gameplayTilemap.WorldToCell(affectedAreaList[k] + tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject areaTile = Instantiate(skillAffectedTilePrefab, affectedAreaList[k] + tileZero, Quaternion.identity);
                playersAffectedTilesList.Add(areaTile);
            }
        }
    }

    #endregion

    #region PlayerPath
    private void ChangePath()
    {
        ClearPath();
        CreatePath();
    }

    private void CreatePath()
    {
        for (int i = 1; i < pathCoordinates.Count; i++)
        {
            GameObject pathObject;
            if (i == pathCoordinates.Count - 1) pathObject = Instantiate(pathEndTileSelectorPrefab, pathCoordinates[i] + tileZero, Quaternion.identity);
            else pathObject = Instantiate(pathTileSelectorPrefab, pathCoordinates[i] + tileZero, Quaternion.identity);
            playerPathList.Add(pathObject);
        }
    }

    private void ClearPath()
    {
        for (int i = 0; i < playerPathList.Count; i++)
        {
            GameObject pathObject = playerPathList[i];
            Destroy(pathObject);
        }
        playerPathList.Clear();
    }
    #endregion

    [TabGroup("For GridAreaMethods Test")]
    [Button("Visualise Attack")]
    public void VisualiseAttack()
    {
        switch (fig)
        {
            case GridAreaMethods.figs.Line:
                VisualizeCoordinateList = GridAreaMethods.CoordinateLine(character, cell, width, maxDistance);
                break;
            case GridAreaMethods.figs.Diagonal:
                VisualizeCoordinateList = GridAreaMethods.DiagonalLine(character, cell, width, maxDistance);
                break;
            case GridAreaMethods.figs.AllCardinalLines:
                VisualizeCoordinateList = GridAreaMethods.AllCardinalLines(character, character, width, maxDistance);
                break;
            case GridAreaMethods.figs.AllDiagonalLines:
                VisualizeCoordinateList = GridAreaMethods.AllDiagonalLines(character, character, width, maxDistance);
                break;
            case GridAreaMethods.figs.Square:
                VisualizeCoordinateList = GridAreaMethods.Perforation(GridAreaMethods.SquareAOE(character, cell, width, true), GridAreaMethods.SquareAOE(character, cell, cutValue, false));
                break;
            case GridAreaMethods.figs.HorseCell:
                VisualizeCoordinateList = GridAreaMethods.HorseCell(character, cell, true);
                break;
            case GridAreaMethods.figs.CircleAOE:
                VisualizeCoordinateList = GridAreaMethods.CircleAOE(character, cell, width);
                break;
        }
        if (VisualizeCoordinateList == null) return;
        for (int i = 0; i < VisualizeCoordinateList.Count; i++)
        {
            Vector3Int tile = gameplayTilemap.WorldToCell(VisualizeCoordinateList[i] + tileZero);
            if (gameplayTilemap.HasTile(tile))
            {
                GameObject attackObject = Instantiate(VisualizeSpritePrefab, VisualizeCoordinateList[i] + tileZero, Quaternion.identity);
                Destroy(attackObject, 1f);
            }
        }
    }
}
