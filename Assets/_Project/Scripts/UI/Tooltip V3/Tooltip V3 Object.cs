using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipV3Object : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject PointerObject;
    private TooltipV3ParentManager tooltip;

    void Start()
    {
        tooltip = GetComponentInChildren<TooltipV3ParentManager>();
        tooltip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter: " + eventData.pointerEnter.name);
        if (eventData.pointerEnter == PointerObject)
        {
            tooltip.ShowTooltip(); 
            //Debug.Log("ShowTooltip!"); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == PointerObject)
        { tooltip.HideTooltip(); }
    }


}
