

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public static class CombatMethods
{

    static void ApplayDamage(CreatorType creatorType, TargetType targetType)
    {

    }

    static void ApplayShield()
    {

    }
    
    static void ApplayHeal()
    {

    }

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////


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

/////////////////////////////////////////////////////////////////////////////////////////////////////////

[Serializable]
class CreatorType //Who cast damage, shield, heal and etc.
{

}

class HeroCreator : CreatorType // Hero, his talents, items, tileEffects, effects
{
    public AllPlayerStats stats;
}

class BossCreator : CreatorType // Boss, his tyleEffects, effects
{

}

class StandartCreator : CreatorType // Items, Minions, nobody TileEffects
{

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////

class TargetType // Who is target for damage, shield, heal and etc.
{

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
