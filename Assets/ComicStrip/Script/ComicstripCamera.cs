using System.Collections;
using UnityEngine;

public class ComicstripCamera : MonoBehaviour
{
    public static ComicstripCamera Instance { get; private set; }

    [System.Serializable]
    public class StripInfo
    {
        public RectTransform strip;  // The strip itself
        public float holdTime = 2f;  // Custom hold time for this strip
    }

    [Header("Strip Settings")]
    [SerializeField] private float zoomScale = 1.2f;
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private StripInfo[] strips; // ✅ Now each strip has its own hold time

    public int ActiveStripIndex { get; private set; } = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(ZoomAndSpotlightSequence());
    }

    private IEnumerator ZoomAndSpotlightSequence()
    {
        for (int i = 0; i < strips.Length; i++)
        {
            ActiveStripIndex = i;

            Vector3 originalScale = strips[i].strip.localScale;
            Vector3 targetScale = originalScale * zoomScale;

            // Zoom in
            float elapsedTime = 0f;
            while (elapsedTime < zoomDuration)
            {
                strips[i].strip.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            strips[i].strip.localScale = targetScale;

            // ✅ Hold for this strip's unique time
            yield return new WaitForSeconds(strips[i].holdTime);

            // Zoom back
            elapsedTime = 0f;
            while (elapsedTime < zoomDuration)
            {
                strips[i].strip.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            strips[i].strip.localScale = originalScale;
        }

        ActiveStripIndex = -1; // Done
    }
}
