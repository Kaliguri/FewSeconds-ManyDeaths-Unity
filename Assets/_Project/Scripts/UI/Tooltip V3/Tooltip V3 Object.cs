using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipV3Object : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipV3Manager tooltip;

    void Start()
    {
        tooltip = GetComponentInChildren<TooltipV3Manager>();
        tooltip.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }


}
