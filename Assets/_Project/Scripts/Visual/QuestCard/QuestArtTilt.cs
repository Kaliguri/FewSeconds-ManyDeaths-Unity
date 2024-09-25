using UnityEngine;

public class QuestArtTilt : MonoBehaviour
{
    [SerializeField]
    float ZOffset = 3000f;
    [SerializeField]
    RectTransform backgroundRectTransform;
    [SerializeField]
    bool isBackgroundRotated = false;

    RectTransform thisRectTransform;
    RectTransform canvasRectTransform;
    Vector3 directionToCursor = -1 * Vector3.forward;
    Vector3 questArtOffset = Vector3.zero;

    void Start()
    {
        thisRectTransform = GetComponent<RectTransform>();
        canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    void Update()
    {
        TiltTowardsCursor();
    }

    void TiltTowardsCursor()
    {
        Vector2 cursorPositionInRect;
        Vector2 thisPositionInRect;
        Vector3 cursorPosition = Input.mousePosition;

        // получение позиции курсора в координатах Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            cursorPosition, 
            null, 
            out cursorPositionInRect);
        // получение позиции Quest Art в координатах Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            thisRectTransform.position, 
            null, 
            out thisPositionInRect);

        directionToCursor.x = -cursorPositionInRect.x;
        directionToCursor.y = -cursorPositionInRect.y;
        directionToCursor.z = ZOffset;
        //directionToCursor = -1 * new Vector3(
        //            cursorPositionInRect.x,
        //            cursorPositionInRect.y,
        //            -ZOffset);
        questArtOffset.x = thisPositionInRect.x;
        questArtOffset.y = thisPositionInRect.y;

        thisRectTransform.forward = directionToCursor;

        //childRectTransform.Rotate(-thisRectTransform.localRotation.eulerAngles);
        if (isBackgroundRotated)
            backgroundRectTransform.localEulerAngles = (-thisRectTransform.localRotation.eulerAngles);
    }
}
