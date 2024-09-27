

using System;
using System.Collections.Generic;

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

[Serializable]
class CreatorType //Who cast damage, shield, heal and etc.
{

}

class HeroCreator : CreatorType // Hero, his talents, items, tileEffects, effects
{

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
