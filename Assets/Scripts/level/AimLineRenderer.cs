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
        Vector3 startPos = playerTransform.position;

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        Vector3 endPos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));        

        endPos.z = 0f;

        startPos = SnapToPixelGrid(startPos);
        endPos = SnapToPixelGrid(endPos);

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        // note: even setting width to 1px doesn't quite render the way I was expecting.
        // good enough for now, but need a more robust, pixel-perfect solution eventually
        lineRenderer.startWidth = 1.0f / pixelsPerUnit;
        lineRenderer.endWidth = 1.0f / pixelsPerUnit;

        lineRenderer.startColor = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        lineRenderer.endColor = new Color(0.0f, 1.0f, 0.0f, 0.0f);
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
