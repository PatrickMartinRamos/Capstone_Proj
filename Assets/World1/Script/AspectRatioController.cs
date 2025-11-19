using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    // Desired reference aspect ratio (width / height)
    private const float TARGET_ASPECT = 1080f / 1920f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateViewport();
    }

#if UNITY_EDITOR
    void Update()
    {
        // Continuously update in Editor for preview resizing
        if (!Application.isPlaying)
            UpdateViewport();
    }
#endif

    void UpdateViewport()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / TARGET_ASPECT;

        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // Add letterbox (top and bottom bars)
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Add pillarbox (left and right bars)
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;
    }
}
