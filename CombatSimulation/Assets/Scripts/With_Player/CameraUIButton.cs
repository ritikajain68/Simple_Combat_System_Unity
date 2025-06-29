using UnityEngine;
using UnityEngine.EventSystems;

public class CameraUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ActionType { PanUp, PanDown, PanLeft, PanRight, ZoomIn, ZoomOut }

    public ActionType action;
    public TopDownCameraController cameraController;

    private bool isPressed = false;

    void Update()
    {
        if (!isPressed || cameraController == null) return;

        switch (action)
        {
            case ActionType.PanUp:
                cameraController.PanUp();
                break;
            case ActionType.PanDown:
                cameraController.PanDown();
                break;
            case ActionType.PanLeft:
                cameraController.PanLeft();
                break;
            case ActionType.PanRight:
                cameraController.PanRight();
                break;
            case ActionType.ZoomIn:
                cameraController.ZoomIn();
                break;
            case ActionType.ZoomOut:
                cameraController.ZoomOut();
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
