using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public Transform centerPoint;
    public float height = 30f;
    public float distance = 20f;

    void LateUpdate()
    {
        if (centerPoint == null) return;

        Vector3 newPos = centerPoint.position + Vector3.up * height - centerPoint.forward * distance;
        transform.position = newPos;
        transform.LookAt(centerPoint.position);
    }
}
