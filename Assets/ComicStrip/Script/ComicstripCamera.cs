using System.Collections;
using UnityEngine;

public class ComicstripCamera : MonoBehaviour
{
    public static ComicstripCamera Instance { get; private set; }

    [System.Serializable]
    public class StripInfo
    {
        public RectTransform strip;
        public float holdTime = 2f;
    }

    [Header("Strip Settings")]
    [SerializeField] private float zoomScale = 1.2f;
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private StripInfo[] strips;

    public int ActiveStripIndex { get; private set; } = -1;
    public StripInfo[] Strips => strips;

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

            // Hold for unique time
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

        ActiveStripIndex = -1;
        Debug.Log("Comic strip sequence completed. Press Play to Proceed to Gameplay");
    }
}
