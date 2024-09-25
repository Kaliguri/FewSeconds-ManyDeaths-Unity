using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : TileEffectScript
{
    public float damage;
    public override void Cast()
    {
        Debug.Log("CastSpikeTileEffect!");
    }
}
