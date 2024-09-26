using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipV3Object : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipV3ParentManager tooltip;

    void Start()
    {
        tooltip = GetComponentInChildren<TooltipV3ParentManager>();
        tooltip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }


}
