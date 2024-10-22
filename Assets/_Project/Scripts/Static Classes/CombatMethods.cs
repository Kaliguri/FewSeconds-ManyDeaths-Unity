

using System;
using Sirenix.OdinInspector;
using UnityEngine;
public static class CombatMethods
{

    public static void ApplayDamage(float value, CombatObject creatorType, CombatObject targetType)
    {
        float modif = creatorType.GetModifiers().DamageModif * (1 - targetType.GetModifiers().DamageResist);
        value *= modif;

        if (value > targetType.GetData().CurrentShield)
        {
            //Debug.Log(targetType.GetPosition());
            if (targetType.GetData().CurrentShield > 0) DamageNumberManager.instance.SpawnShieldChangeText(targetType.GetPosition(), value.ToString());
            value -= targetType.GetData().CurrentShield;
            targetType.GetData().CurrentShield = 0;

            DamageNumberManager.instance.SpawnDamageText(targetType.GetPosition(), value.ToString());
            targetType.GetData().CurrentHP -= value;
            if (targetType.GetData().CurrentHP < 0) targetType.GetData().CurrentHP = 0;
        }
        else
        {
            DamageNumberManager.instance.SpawnShieldChangeText(targetType.GetPosition(), value.ToString());
            targetType.GetData().CurrentShield -= value;
        }

        if (targetType is HeroCombatObject)
        {   
            GlobalEventSystem.SendPlayerShieldChanged();
            GlobalEventSystem.SendPlayerHPChanged();
        }
        else if (targetType is BossCombatObject)
        {
            GlobalEventSystem.SendBossHPChanged();
        }
    }
    
    public static void ApplayHeal(float value, CombatObject creatorType, CombatObject targetType)
    {        
        float modif = creatorType.GetModifiers().HealModif * (1 - targetType.GetModifiers().HealEffectiveResist);
        value *= modif;

        DamageNumberManager.instance.SpawnHealText(targetType.GetPosition(), value.ToString());
        if (targetType.GetData().CurrentHP + value >= targetType.GetData().MaxHP) targetType.GetData().CurrentHP = targetType.GetData().MaxHP;
        else targetType.GetData().CurrentHP += value;

        if (targetType is HeroCombatObject)
        {   
            GlobalEventSystem.SendPlayerHPChanged();
        }
        else if (targetType is BossCombatObject)
        {
            GlobalEventSystem.SendBossHPChanged();
        }
    }

    public static void ApplayShield(float value, CombatObject creatorType, CombatObject targetType)
    {
        float modif = creatorType.GetModifiers().ShieldModif * (1 - targetType.GetModifiers().ShieldEffectiveResist);
        value *= modif;

        DamageNumberManager.instance.SpawnShieldChangeText(targetType.GetPosition(), value.ToString());
        targetType.GetData().CurrentShield += value;

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

    public GameObject gameObject;
    public Vector3 position;

    public virtual CombatStats GetData()
    {
        return Data;
    } 
    public virtual Modifiers GetModifiers()
    {
        return Modifiers;
    }

    public virtual Vector3 GetPosition()
    {
        return position;
    }
}

public class HeroCombatObject: CombatObject // Hero, his talents, items, tileEffects, effects
{
    //public new int ObjectID; //Player ID

    private CombatPlayerDataInStage playersData;
    private AllPlayerStats stats => playersData._TotalStatsList[ObjectID];

    public new Modifiers Modifiers => stats.general.Modifiers;
    public new CombatStats Data => stats.currentCombat;

    public new GameObject gameObject => playersData.PlayersHeroes[ObjectID];
    public new Vector3 position => gameObject.transform.position;

    public override CombatStats GetData()
    {
        return Data;
    } 
    public override Modifiers GetModifiers()
    {
        return Modifiers;
    } 
    public override Vector3 GetPosition()
    {
        return position;
    }

    public HeroCombatObject(int objectID, CombatPlayerDataInStage combatPlayerDataInStage)
    {
        ObjectID = objectID;
        playersData = combatPlayerDataInStage;
    }
}

public class BossCombatObject : CombatObject // Boss, his tyleEffects, effects
{
    private BossManager bossManager;
    public new CombatStats Data => bossManager.bossStats;
    public new Modifiers Modifiers => new Modifiers();

    public new GameObject gameObject => bossManager.BossGameObject;
    public new Vector3 position => gameObject.transform.position;


    public override Modifiers GetModifiers()
    {
        return Modifiers;
    }
    public BossCombatObject(BossManager _bossManager)
    {
        bossManager = _bossManager;
    }

    public override CombatStats GetData()
    {
        return Data;
    }
    public override Vector3 GetPosition()
    {
        return position;
    }
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
    public float MaxHP = 0;
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