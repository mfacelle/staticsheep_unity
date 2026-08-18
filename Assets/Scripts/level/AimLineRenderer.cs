using UnityEngine;
using UnityEngine.InputSystem;


public class AimLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float pixelsPerUnit = 16.0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Get character position
        Vector3 startPos = playerTransform.position;


        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        Vector3 endPos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));        
        // Vector2 playerPos = PlayerObject.transform.position;

        // Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPos.z = 0f;

        startPos = SnapToPixelGrid(startPos);
        endPos = SnapToPixelGrid(endPos);

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        lineRenderer.startWidth = 1.0f / pixelsPerUnit;
        lineRenderer.endWidth = 1.0f / pixelsPerUnit;

        lineRenderer.startColor = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        lineRenderer.endColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
    }

    Vector3 SnapToPixelGrid(Vector3 worldPos)
    {
        return new Vector3(
            Mathf.Round(worldPos.x * pixelsPerUnit) / pixelsPerUnit,
            Mathf.Round(worldPos.y * pixelsPerUnit) / pixelsPerUnit,
            0f
        );
    }
}
