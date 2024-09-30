

using System;
using Sirenix.OdinInspector;
using UnityEngine;

public static class CombatMethods
{

    public static void ApplayDamage(float damageValue, CombatObject creatorType, CombatObject targetType)
    {
        float damageModif = creatorType.GetModifiers().DamageModif * (1 - targetType.GetModifiers().DamageResist);
        damageValue *= damageModif;

        if (damageValue > targetType.GetData().CurrentShield)
        {
            damageValue -= targetType.GetData().CurrentShield;
            targetType.GetData().CurrentShield = 0;
            targetType.GetData().CurrentHP -= damageValue;
        }
        else
        {
            targetType.GetData().CurrentShield -= damageValue;
        }

        if (targetType is HeroCombatObject)
        {   
            GlobalEventSystem.SendPlayerShieldChanged();
            GlobalEventSystem.SendPlayerHPChanged();
        }
    }
    
    public static void ApplayHeal(float healValue, CombatObject creatorType, CombatObject targetType)
    {        
        float healModif = creatorType.GetModifiers().HealModif * (1 - targetType.GetModifiers().HealEffectiveResist);
        healValue *= healModif;

        targetType.GetData().CurrentHP += healValue;

        if (targetType is HeroCombatObject)
        {   
            GlobalEventSystem.SendPlayerHPChanged();
        }
        
    }

    public static void ApplayShield(float shieldValue, CombatObject creatorType, CombatObject targetType)
    {
        float shieldModif = creatorType.GetModifiers().ShieldModif * (1 - targetType.GetModifiers().ShieldEffectiveResist);
        shieldValue *= shieldModif;

        targetType.GetData().CurrentShield += shieldValue;

        if (targetType is HeroCombatObject)
        {   
            GlobalEventSystem.SendPlayerShieldChanged();
        }
    }
}
#region LegacyTypes
/////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
[Serializable]
class CreatorType //Who cast damage, shield, heal and etc.
{
    public int CreatorID = 0;
    public CombatStats CombatStats = new CombatStats();
    public Modifiers Modifiers = new Modifiers();
}

class HeroCreator : CreatorType // Hero, his talents, items, tileEffects, effects
{
    public new int CreatorID; //Player ID
    public new CombatStats CombatStats => stats.currentCombat;
    public new Modifiers Modifiers => stats.general.Modifiers;

    private AllPlayerStats stats;
}

class BossCreator : CreatorType // Boss, his tyleEffects, effects
{

}

class StandartCreator : CreatorType // Items, Minions, nobody TileEffects
{

}
*/
/////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
class TargetType // Who is target for damage, shield, heal and etc.
{
    public CombatStats CombatStats = new CombatStats();
}

class HeroTarget : TargetType
{

}

class BossTarget : TargetType
{
    
}

class StandartTarget : TargetType // Terrain, minion and etc.
{
    
}

*/

/////////////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

[Serializable]
public class CombatObject //Who cast damage, shield, heal and etc.
{
    public int ObjectID = 0;
    public Modifiers Modifiers;
    public CombatStats Data;

    public virtual CombatStats GetData()
    {
        return Data;
    } 
    public virtual Modifiers GetModifiers()
    {
        return Modifiers;
    } 
     

}

public class HeroCombatObject: CombatObject // Hero, his talents, items, tileEffects, effects
{
    //public new int ObjectID; //Player ID

    private CombatPlayerDataInStage playersData;
    private AllPlayerStats stats => playersData._TotalStatsList[ObjectID];

    public new Modifiers Modifiers => stats.general.Modifiers;
    public new CombatStats Data => stats.currentCombat;

    public override CombatStats GetData()
    {
        return Data;
    } 
    public override Modifiers GetModifiers()
    {
        return Modifiers;
    } 

    public HeroCombatObject(int objectID, CombatPlayerDataInStage combatPlayerDataInStage)
    {
        ObjectID = objectID;
        playersData = combatPlayerDataInStage;
        Debug.Log(playersData._TotalStatsList[ObjectID].general.Modifiers.DamageModif);
    }

    
}

public class BossCombatObject : CombatObject // Boss, his tyleEffects, effects
{

}

public class StandartCombatObject : CombatObject // Items, Minions, nobody TileEffects
{

}

[Serializable]
public class Modifiers
{
    [Title("Resist")] 
    public float DamageResist = 0f;
    public float ShieldEffectiveResist = 0f;
    public float HealEffectiveResist = 0f;

    [Title("Cast Modifiers")] 
    public float DamageModif = 1f;
    public float ShieldModif = 1f;
    public float HealModif = 1f;
}

[Serializable]
public class CombatStats
{
    public float CurrentHP = 0;
    public float CurrentShield = 0;
    public int CurrentEnergy = 0;
}


[Serializable]
public class GeneralPlayerStats
{
    [Title("HP")]
    public float MaxHP = 0;
    public float StartHP = 0;

    [Title("Energy")]
    public int MaxEnergy;
    public int EnergyPerTurn;

    [Title("Skill")]
    public float AbilityPower;

    [Title("Modifiers")]
    public Modifiers Modifiers = new Modifiers();

    [Title("Level")]
    public int Level = 1;
    public float HPIncreaseFromLVLUP = 0.2f;
}
[Serializable]
public class AllPlayerStats
{
    [Title("Combat (Current...)")]
    public CombatStats currentCombat = new CombatStats();

    [Title("General")]
    public GeneralPlayerStats general = new GeneralPlayerStats();
}