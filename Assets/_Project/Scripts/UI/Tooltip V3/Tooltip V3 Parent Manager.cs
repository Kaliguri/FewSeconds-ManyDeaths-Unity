using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class TooltipV3ParentManager : MonoBehaviour
{
    [Title("Tooltip V3 Parent")]
    [Title("Gameobject Reference")]
    [SerializeField] GameObject TooltipContent;
    [SerializeField] protected GameObject Background;

    [Title("Edge Detecting Settings")]
    //[SerializeField] bool IsCameraSpace = true;
    [SerializeField] float IndentX = 0.1f;
    [SerializeField] float IndentY = 0.1f;

    [Title("Tooltip Style")]
    public TagFontStyle Style;

    

    private RectTransform CanvasRect => transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    public Camera cameraBrain => FindObjectOfType<Camera>().GetComponent<Camera>();
    public virtual void Refresh()
    {
        Debug.Log("RefreshTooltip");
    }

    protected void TooltipContentShow()
    {
        TooltipContent.gameObject.SetActive(true);
    }
    public virtual void ShowTooltip()
    {
        Debug.Log("ShowTooltip");
    }

    public virtual void HideTooltip()
    {
        Debug.Log("HideTooltip");
    }

    protected void EdgeDetection()
    {
        var backgroundRect = Background.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        backgroundRect.GetWorldCorners(corners);

        // Определяем минимальные и максимальные координаты
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        // Получаем размеры экрана в мировых координатах
        Vector3 bottomLeft = cameraBrain.ScreenToWorldPoint(new Vector3(0, 0, cameraBrain.nearClipPlane));
        Vector3 topRight = cameraBrain.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraBrain.nearClipPlane));

        float screenMinX = bottomLeft.x;
        float screenMaxX = topRight.x;
        float screenMinY = bottomLeft.y;
        float screenMaxY = topRight.y;

        var tooltipTransform = gameObject.GetComponent<RectTransform>();

        // Корректируем положение по X
        if (minX < screenMinX + IndentX)
        {
            tooltipTransform.position += new Vector3(screenMinX + IndentX - minX, 0, 0);
        }
        else if (maxX > screenMaxX - IndentX)
        {
            tooltipTransform.position += new Vector3(screenMaxX - IndentX - maxX, 0, 0);
        }

        // Корректируем положение по Y
        if (minY < screenMinY + IndentY)
        {
            tooltipTransform.position += new Vector3(0, screenMinY + IndentY - minY, 0);
        }
        else if (maxY > screenMaxY - IndentY)
        {
            tooltipTransform.position += new Vector3(0, screenMaxY - IndentY - maxY, 0);
        }
    }

}
