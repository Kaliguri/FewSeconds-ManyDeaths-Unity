using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{

    [Title("Prefabs")]
    [SerializeField] GameObject prefabParentObject;

    [SerializeField] List<DamageNumber> prefabsList = new();

    public static DamageNumberManager instance = null;
    void Awake()
    {
        if (instance == null) {instance = this;}
    }

    void Start()
    {
        //variantsListInizialize();
        prefabParentObject.SetActive(false);
    }

    void TextSpawn(int prefabID, Vector3 position, string textNumber, float scale = 1f, bool IsText = false)
    {
        if (prefabsList.Count >= prefabID)
        {

        DamageNumber damageNumber = prefabsList[prefabID].Spawn(position, textNumber); 
        damageNumber.transform.localScale *= scale;

        if (IsText)
            {
                damageNumber.enableNumber = false;
                damageNumber.leftText = textNumber;
            }
        }

        else Debug.LogError("Not prefab for Damage Number Manager!");
    }

    string AddPlusOrMinus(string value)
    {
        var intValue = int.Parse(value);

        if (intValue > 0) 
        return "+" + value;

        else if (intValue < 0) 
        return "-" + value;

        else
        return value;
    }


    public void SpawnDamageText(Vector3 position, string textNumber, float scale = 1f)
    {
        TextSpawn(0, position, textNumber, scale);
    }
    public void SpawnShieldChangeText(Vector3 position, string textNumber, float scale = 1f)
    {   
        TextSpawn(1, position, AddPlusOrMinus(textNumber), scale);
    }
    public void SpawnHealText(Vector3 position, string textNumber, float scale = 1f)
    {
        TextSpawn(2, position, textNumber, scale);
    }
    public void SpawnAmmoText(Vector3 position, string textNumber, float scale = 1f)
    {
        TextSpawn(3, position, AddPlusOrMinus(textNumber), scale);
    }
    public void SpawnInfoText(Vector3 position, string textNumber, float scale = 1f)
    {
        TextSpawn(4, position, textNumber, scale, IsText: true);
    }
}
