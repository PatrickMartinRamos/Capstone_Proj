using System.Collections;
using UnityEngine;

public class ComicstripCamera : MonoBehaviour
{
    [Header("Strip Settings")]
    [SerializeField] private float zoomScale = 1.2f;     // How much to scale
    [SerializeField] private float zoomDuration = 0.5f; // Time to zoom in
    [SerializeField] private float holdTime = 2f;       // Time to hold before next strip

    [SerializeField] private RectTransform[] strips;

    private void Start()
    {
        // Start zoom coroutine
        StartCoroutine(ZoomStripsOneByOne());
    }

    private IEnumerator ZoomStripsOneByOne()
    {
        foreach (var strip in strips)
        {
            if (strip == transform) continue; // Skip parent itself

            Vector3 originalScale = strip.localScale;
            Vector3 targetScale = originalScale * zoomScale;

            // Zoom in smoothly
            float elapsedTime = 0f;
            while (elapsedTime < zoomDuration)
            {
                strip.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            strip.localScale = targetScale;

            // Hold for a while
            yield return new WaitForSeconds(holdTime);

            // Zoom back to original scale
            elapsedTime = 0f;
            while (elapsedTime < zoomDuration)
            {
                strip.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            strip.localScale = originalScale;
        }
    }
}
