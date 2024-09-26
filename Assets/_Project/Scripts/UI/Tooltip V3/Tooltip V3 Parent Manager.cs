using UnityEngine;

public class TooltipV3ParentManager : MonoBehaviour
{
    public virtual void Refresh()
    {
        Debug.Log("RefreshTooltip");
    }
    public virtual void ShowTooltip()
    {
        Debug.Log("ShowTooltip");
    }

    public virtual void HideTooltip()
    {
        Debug.Log("HideTooltip");
    }

}
