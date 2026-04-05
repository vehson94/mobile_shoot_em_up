using UnityEngine;
using UnityEngine.Events;

public class ScanButton : MonoBehaviour
{
    public UnityEvent onClick;

    private Camera mainCamera;
    private Collider2D hitCollider2D;
    private Collider hitCollider3D;
    private Renderer cachedRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;
        hitCollider2D = GetComponent<Collider2D>();
        hitCollider3D = GetComponent<Collider>();
        cachedRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int index = 0; index < Input.touchCount; index++)
            {
                Touch touch = Input.GetTouch(index);
                if (touch.phase == TouchPhase.Began && IsPointerInside(touch.position))
                {
                    onClick?.Invoke();
                    return;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && IsPointerInside(Input.mousePosition))
        {
            onClick?.Invoke();
        }
    }

    private bool IsPointerInside(Vector2 screenPosition)
    {
        Camera activeCamera = GetActiveCamera();
        if (activeCamera == null)
        {
            return false;
        }

        if (hitCollider2D != null)
        {
            Vector3 worldPoint = ScreenToWorldPoint(activeCamera, screenPosition);
            return hitCollider2D.OverlapPoint(worldPoint);
        }

        if (hitCollider3D != null)
        {
            Vector3 worldPoint = ScreenToWorldPoint(activeCamera, screenPosition);
            Vector3 closestPoint = hitCollider3D.ClosestPoint(worldPoint);
            return (closestPoint - worldPoint).sqrMagnitude <= Mathf.Epsilon;
        }

        if (cachedRenderer != null)
        {
            Vector3 worldPoint = ScreenToWorldPoint(activeCamera, screenPosition);
            worldPoint.z = cachedRenderer.bounds.center.z;
            return cachedRenderer.bounds.Contains(worldPoint);
        }

        return false;
    }

    private Camera GetActiveCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        return mainCamera;
    }

    private Vector3 ScreenToWorldPoint(Camera activeCamera, Vector2 screenPosition)
    {
        float depth = activeCamera.WorldToScreenPoint(transform.position).z;
        return activeCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, depth));
    }

    private void OnButtonClick()
    {
        Debug.Log("Scan button clicked!");
    }
}
