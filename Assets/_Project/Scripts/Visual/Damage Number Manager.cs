using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    /*
    [Title("Variants")]
    public DamageNumber Damage;
    public DamageNumber Heal;
    public DamageNumber Shield;
    */

    [Title("Prefabs")]
    [SerializeField] GameObject prefabParentObject;

    private List<DamageNumber> prefabsList = new();
    //private List<DamageNumber> variantsList = new();
    //private Vector2 tileZero => MapClass.instance.tileZero;

    public static DamageNumberManager instance = null;
    void Awake()
    {
        if (instance == null) {instance = this;}
    }

    void Start()
    {
        GetDamageNumberPrefabs();
        //variantsListInizialize();
        prefabParentObject.SetActive(false);
    }

    void GetDamageNumberPrefabs()
    {
        var parentTransform = prefabParentObject.transform;
        for(int n = 0; n < parentTransform.childCount; n++)
        {
            //Debug.Log(prefabsList + " " + parentTransform.GetChild(n).GetComponent<DamageNumber>());
            prefabsList.Add(parentTransform.GetChild(n).GetComponent<DamageNumber>());
        }
    }

    /*void variantsListInizialize()
    {
        Damage = prefabsList[0];
        Heal = prefabsList[0];
        Shield = prefabsList[0];
    }*/

    public void Spawn(int prefabID, Vector3 position, float number)
    {
        DamageNumber damageNumber = prefabsList[prefabID].Spawn(position, number); 
    }
}
