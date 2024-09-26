using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridVisualizer : MonoBehaviour
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

    [TabGroup("For CombatGridMethods Test")]
    [SerializeField] GameObject VisualizeSpritePrefab;

    [TabGroup("For CombatGridMethods Test")]
    [SerializeField] CombatGridMethods.figs fig;

    [TabGroup("For CombatGridMethods Test")]
    [SerializeField] Vector2 character, cell;

    [TabGroup("For CombatGridMethods Test")]
    [SerializeField] int width = 1, maxDistance = 5, cutValue = 0;
    [TabGroup("For CombatGridMethods Test")]
    public List<Vector2> VisualizeCoordinateList = new();


    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private Tilemap gameplayTilemap => mapClass.gameplayTilemap;
    private Vector2 tileZero => mapClass.tileZero;
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
    private Vector2 castPosition => playerMovementController.LastPosition;

    private List<GameObject> pathList = new();
    private List<GameObject> availableTileGameObjectList = new();
    private List<GameObject> playersTilesGameObjectList = new();
    private List<GameObject> playersAffectedTilesList = new();
    private GameObject tileSelector;
    private GameObject tileSkillSelector;
    private InputActions inputActions;
    private bool isPlayerCasting = false;

    void Awake()
    {
        inputActions = new InputActions();

        GlobalEventSystem.StartCombat.AddListener(CreatePlayersTiles);
        GlobalEventSystem.PlayerStartMove.AddListener(HidePlayersTiles);
        GlobalEventSystem.PlayerEndMove.AddListener(ShowPlayersTiles);
        GlobalEventSystem.PlayerActionChoosed.AddListener(PlayerActionChoosed);
        GlobalEventSystem.PlayerActionUpdate.AddListener(PlayerActionUpdate);
        GlobalEventSystem.PlayerActionUnchoosed.AddListener(PlayerActionUnchoosed);
        GlobalEventSystem.PlayerActionAproved.AddListener(PlayerActionUnchoosed);
        GlobalEventSystem.PathChanged.AddListener(ChangePath);
        GlobalEventSystem.ResultStageStarted.AddListener(SendClearAprovedAffectedAreas);
    }

    private void SendClearAprovedAffectedAreas(int arg0)
    {
        ClearAprovedAffectedAreas();
    }

    void Start()
    {
        SetVisualizeCoordinateList();
        inputActions.Enable();
    }

    private void FixedUpdate()
    {
        UpdateTileSelector();
    }

    private void CreatePlayersTiles()
    {
        for (int i = 0; i < heroPositions.Length; i++)
        {
            GameObject playerTile = Instantiate(heroTile, heroPositions[i] + mapClass.tileZero, Quaternion.identity);

            Color color = playerInfoData.ColorList[i];
            playerTile.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, playerTile.GetComponent<SpriteRenderer>().color.a);

            playersTilesGameObjectList.Add(playerTile);
        }
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
            playersTilesGameObjectList[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void PlayerActionUpdate()
    {
        UpdateAprovedAffectedAreas();
        UpdateSkillAvaliableArea();
    }

    private void ChangePath()
    {
        ClearPath();
        CreatePath();
    }

    private void CreatePath()
    {
        for (int i = 0; i < pathCoordinates.Count; i++)
        {
            GameObject pathObject;
            if (i == pathCoordinates.Count - 1) pathObject = Instantiate(pathEndTilePrefab, pathCoordinates[i] + tileZero, Quaternion.identity);
            else pathObject = Instantiate(pathTilePrefab, pathCoordinates[i] + tileZero, Quaternion.identity);
            pathList.Add(pathObject);
        }
    }

    private void ClearPath()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            GameObject pathObject = pathList[i];
            Destroy(pathObject);
        }
        pathList.Clear();
    }

    private void PlayerActionUnchoosed()
    {
        UpdateAprovedAffectedAreas();
        isPlayerCasting = false;
        ClearAvaliableArea();
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
        List<Vector2> affectedAreaList = skill.Area(castPosition, TargetTileList[i][skillIndex], skillIndex);
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

    private void ClearAprovedAffectedAreas()
    {
        for (int i = 0; i < playersAffectedTilesList.Count; i++)
        {
            GameObject tile = playersAffectedTilesList[i];
            Destroy(tile);
        }
        playersAffectedTilesList.Clear();
    }

    private void PlayerActionChoosed()
    {
        isPlayerCasting = true;
        UpdateSkillAvaliableArea();
    }

    private void UpdateSkillAvaliableArea()
    {
        if (skillSelected.HasConditionsForSelectedCell)
        {
            ClearAvaliableArea();
            List<Vector2> availableTilesList = skillSelected.AvailableTiles(castPosition, targetPoints);
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
            MapClass.TileStates tileState = mapClass.TileState(TileCenter - tileZero);

            if (isPlayerCasting) UpdateSkillAffectedArea(TileCenter);
            else
            {
                if (tileState == MapClass.TileStates.Player) tileSelector = Instantiate(allyTypeTileSelector, TileCenter, Quaternion.identity);
                else if (tileState == MapClass.TileStates.Boss) tileSelector = Instantiate(enemyTypeTileSelector, TileCenter, Quaternion.identity);
                else if (tileState == MapClass.TileStates.Terrain) tileSelector = Instantiate(terrainTypeTileSelector, TileCenter, Quaternion.identity);
                else tileSelector = Instantiate(standartTileSelector, TileCenter, Quaternion.identity);
            }
        }
    }

    private void UpdateSkillAffectedArea(Vector2 TileCenter)
    {
        List<Vector2> affectedAreaList = skillSelected.Area(castPosition, TileCenter - tileZero, targetPoints);
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

    [TabGroup("For CombatGridMethods Test")]
    [Button("Visualise Attack")]
    public void VisualiseAttack()
    {
        switch (fig)
        {
            case CombatGridMethods.figs.Line:
                VisualizeCoordinateList = CombatGridMethods.CoordinateLine(character, cell, width,maxDistance);
                break;
            case CombatGridMethods.figs.Diagonal:
                VisualizeCoordinateList = CombatGridMethods.DiagonalLine(character, cell, width, maxDistance);
                break;
            case CombatGridMethods.figs.AllCardinalLines:
                VisualizeCoordinateList = CombatGridMethods.AllCardinalLines(character, width, maxDistance);
                break;
            case CombatGridMethods.figs.AllDiagonalLines:
                VisualizeCoordinateList = CombatGridMethods.AllDiagonalLines(character, width, maxDistance);
                break;
            case CombatGridMethods.figs.Square:
                VisualizeCoordinateList = CombatGridMethods.Perforation( CombatGridMethods.SquareAOE(character, cell, width, true), CombatGridMethods.SquareAOE(character, cell, cutValue, false));
                break;
            case CombatGridMethods.figs.HorseCell:
                VisualizeCoordinateList = CombatGridMethods.HorseCell(character, cell, true);
                break;
            case CombatGridMethods.figs.CircleAOE:
                VisualizeCoordinateList = CombatGridMethods.CircleAOE(character, cell, width);
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

    public void SetVisualizeCoordinateList()
    {
        VisualizeCoordinateList = new List<Vector2>{new Vector2(1, 1), new Vector2(1,2)};
    }
}
