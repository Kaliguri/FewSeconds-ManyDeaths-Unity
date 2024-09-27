using UnityEngine;

public class TileEffectScript
{
    [SerializeField] private bool IsCastOnEnter;
    [SerializeField] private bool IsCastOnExit;
    [SerializeField] private bool IsCastOnStay;
    public virtual void Cast()
    {
        Debug.Log("CastTileEffect to player ");
    }
}
