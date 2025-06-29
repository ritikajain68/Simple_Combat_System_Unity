using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 10f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        cam.orthographic = true;
        transform.position = new Vector3(4.42f, 30f, 0.32f);
        transform.rotation = new Quaternion(0.7071068f, 0f, -0.000000052f, 0.7071068f);
    }

    private void Update()
    {
        HandleKeyboardInput();
    }

    public void PanUp()
    {
        transform.position += Vector3.forward * panSpeed * Time.deltaTime;
    }

    public void PanDown()
    {
        transform.position += Vector3.back * panSpeed * Time.deltaTime;
    }

    public void PanLeft()
    {
        transform.position += Vector3.left * panSpeed * Time.deltaTime;
    }

    public void PanRight()
    {
        transform.position += Vector3.right * panSpeed * Time.deltaTime;
    }

    public void ZoomIn()
    {
        cam.orthographicSize = Mathf.Max(minZoom, cam.orthographicSize - zoomSpeed * Time.deltaTime);
    }

    public void ZoomOut()
    {
        cam.orthographicSize = Mathf.Min(maxZoom, cam.orthographicSize + zoomSpeed * Time.deltaTime);
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            PanUp();
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            PanDown();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            PanLeft();
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            PanRight();

        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus)) // Zoom in with = or Numpad +
            ZoomIn();
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) // Zoom out with - or Numpad -
            ZoomOut();
    }
}