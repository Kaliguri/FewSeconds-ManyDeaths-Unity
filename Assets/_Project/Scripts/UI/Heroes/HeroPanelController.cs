using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroPanelController : MonoBehaviour
{
    [SerializeField] private List<HeroData> attackersHeroList;
    [SerializeField] private List<HeroData> defendersHeroList;
    [SerializeField] private List<HeroData> healersHeroList;
    [SerializeField] private List<HeroData> strategistHeroList;
    [SerializeField] private List<HeroData> universalHeroList;

    private List<List<HeroData>> listHeroList;
    
    [Header("Object Reference")]
    [SerializeField] private GameObject attackersPanelObj;
    [SerializeField] private GameObject defendersPanelObj;
    [SerializeField] private GameObject healersPanelObj;
    [SerializeField] private GameObject strategistPanelObj;
    [SerializeField] private GameObject universalPanelObj;

    private List<GameObject> panelObjList;

    void Start()
    {
        FillLists();
        InizializePanel();
    }

    void FillLists()
    {
        listHeroList = new List<List<HeroData>> {attackersHeroList, defendersHeroList, healersHeroList, strategistHeroList, universalHeroList};
        panelObjList = new List<GameObject> {attackersPanelObj, defendersPanelObj, healersPanelObj, strategistPanelObj, universalPanelObj};
    }
    void InizializePanel()
    {
        for (int numberList = 0; numberList < listHeroList.Count; numberList++)
        {
            var heroDataList = listHeroList[numberList];
            var panelIconList = panelObjList[numberList].GetComponent<TypeHeroInHeroPanelConrtoller>().HeroIcons;

            for (int numberData = 0; numberData < heroDataList.Count; numberData++)
            {
                panelIconList[numberData].GetComponent<HeroChoiceButtonController>().DataTranfer(heroDataList[numberData]);
            }
            
        }
    }
}
