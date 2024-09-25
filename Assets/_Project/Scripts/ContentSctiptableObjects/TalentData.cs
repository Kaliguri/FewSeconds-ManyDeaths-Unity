
using UnityEngine;


[CreateAssetMenu(fileName = "TalentData", menuName = "FewSecondsManyDeaths/HeroData/TalentData")]
public class TalentData : ScriptableObject
{
    [Header("General")]
    public string Name;
    public string Type;
    [TextArea] public string Description;

    [Header("Icon")]
    public Sprite SkillIcon;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public TalentScript TalentScript = new TalentScript();
}
