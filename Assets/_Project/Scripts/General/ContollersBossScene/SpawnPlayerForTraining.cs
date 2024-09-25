using UnityEngine;

public class SpawnPlayerForTraining : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Vector2[] SpawnCoordinate;
    private Vector2 zeroPoint => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().tileZero;

    void Start()
    {
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        Vector2 Coordinates = SpawnCoordinate[0] + zeroPoint;
        GameObject player = Instantiate(Player, Coordinates, Quaternion.identity);

        GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().ChangeCell(SpawnCoordinate[0], MapClass.TileStates.Player);

    }
}
