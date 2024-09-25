using Sirenix.OdinInspector;
using UnityEngine;


public class SessionSeed : MonoBehaviour
{
    public int Seed;

    [Header("Settings")]
    public int Lenght = 8;

    [Button("GenerateSeed")]
    public void GenerateSeed()
    {
        int minSeed = (int)System.Math.Pow(10, Lenght - 1);
        int maxSeed = (int)System.Math.Pow(10, Lenght);
        //Debug.Log(minSeed + " " + maxSeed);
        
        Seed = Random.Range (minSeed, maxSeed);

        SetSeed();
    }

    [Button("SetSeed")]
    public void SetSeed()
    {
        Random.InitState(Seed);
    }
}
