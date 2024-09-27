

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public static class CombatMethods
{

    static void ApplayDamage(CombatObject creatorType, CombatObject targetType)
    {

    }

    static void ApplayShield(CombatObject creatorType, CombatObject targetType)
    {

    }
    
    static void ApplayHeal(CombatObject creatorType, CombatObject targetType)
    {

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
class CombatObject //Who cast damage, shield, heal and etc.
{
    public int CreatorID = 0;
    public CombatStats CombatStats = new CombatStats();
    public Modifiers Modifiers = new Modifiers();
}

class HeroCombatObject: CombatObject // Hero, his talents, items, tileEffects, effects
{
    public new int CreatorID; //Player ID
    public new CombatStats CombatStats => stats.currentCombat;
    public new Modifiers Modifiers => stats.general.Modifiers;

    private AllPlayerStats stats;
}

class BossCombatObject : CombatObject // Boss, his tyleEffects, effects
{

}

class StandartCombatObject : CombatObject // Items, Minions, nobody TileEffects
{

}

public class Modifiers
{
    [Title("Resist")] 
    public float DamageResist = 0f;
    public float ShieldResist = 0f;
    public float HealResist = 0f;

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
    public Modifiers Modifiers;

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