using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripSpotLight : MonoBehaviour
{
    [Header("Assign Strip Parents")]
    [SerializeField] private Transform[] stripParents; // Assign each strip parent in Inspector
    [SerializeField] private string spotlightLayerName = "spot_light";
    [SerializeField] private float spotlightTime = 2f;

    private List<StripData> strips = new List<StripData>();

    private void Awake()
    {
        // Collect all strips & store their original layer names
        foreach (Transform stripParent in stripParents)
        {
            SpriteRenderer[] renderers = stripParent.GetComponentsInChildren<SpriteRenderer>(true);

            if (renderers.Length > 0)
            {
                string originalLayer = renderers[0].sortingLayerName; // Assume all share same layer
                strips.Add(new StripData(renderers, originalLayer));
            }
            else
            {
                Debug.LogWarning($"No SpriteRenderers found in {stripParent.name}");
            }
        }
    }

    private void Start()
    {
        StartCoroutine(CycleSpotlights());
    }

    private IEnumerator CycleSpotlights()
    {
        if (strips.Count == 0)
        {
            Debug.LogWarning("No strips found for spotlight.");
            yield break;
        }

        while (true)
        {
            foreach (var strip in strips)
            {
                // Reset all strips to their original layers
                ResetAllStrips();

                // Spotlight current strip
                foreach (var sr in strip.Renderers)
                {
                    if (sr != null)
                        sr.sortingLayerName = spotlightLayerName;
                }

                // Wait before switching
                yield return new WaitForSeconds(spotlightTime);
            }
        }
    }

    private void ResetAllStrips()
    {
        foreach (var strip in strips)
        {
            foreach (var sr in strip.Renderers)
            {
                if (sr != null)
                    sr.sortingLayerName = strip.OriginalLayer;
            }
        }
    }

    [System.Serializable]
    private class StripData
    {
        public SpriteRenderer[] Renderers;
        public string OriginalLayer;

        public StripData(SpriteRenderer[] renderers, string originalLayer)
        {
            Renderers = renderers;
            OriginalLayer = originalLayer;
        }
    }
}
