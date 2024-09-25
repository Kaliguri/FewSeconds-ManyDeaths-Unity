using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "SkillData", menuName = "FewSecondsManyDeaths/HeroData/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("General")]
    public LocalizedString Name;
    public LocalizedString Description;

    [Header("Icon")]
    public Sprite SkillIcon;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public SkillScript SkillScript = new SkillScript();

}
