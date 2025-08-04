using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripSpotLight : MonoBehaviour
{
    [Header("Assign Strip Parents (each must have a SpriteMask)")]
    [SerializeField] private Transform[] stripParents; 
    private string spotlightLayerName = "spot_light";
    private string spotlightMaskFront = "spot_light"; 
    private string spotlightMaskBack = "spot_light"; 
    [SerializeField] private float nonSpotlightAnimSpeed = 0.2f;

    private List<StripData> strips = new List<StripData>();

    private void Awake()
    {
        foreach (Transform stripParent in stripParents)
        {
            SpriteRenderer[] renderers = stripParent.GetComponentsInChildren<SpriteRenderer>(true);
            SpriteMask mask = stripParent.GetComponentInChildren<SpriteMask>(true);
            Animator[] animators = stripParent.GetComponentsInChildren<Animator>(true);

            if (renderers.Length > 0)
            {
                string originalLayer = renderers[0].sortingLayerName;
                int originalFront = mask ? mask.frontSortingLayerID : 0;
                int originalBack = mask ? mask.backSortingLayerID : 0;

                strips.Add(new StripData(renderers, originalLayer, mask, originalFront, originalBack, animators));
            }
        }
    }

    private void Update()
    {
        int activeIndex = ComicstripCamera.Instance.ActiveStripIndex;
        UpdateSpotlight(activeIndex);
    }

    private void UpdateSpotlight(int activeIndex)
    {
        for (int i = 0; i < strips.Count; i++)
        {
            StripData strip = strips[i];
            bool isActive = (i == activeIndex);

            // Sorting layers
            foreach (var sr in strip.Renderers)
                sr.sortingLayerName = isActive ? spotlightLayerName : strip.OriginalLayer;

            // Mask
            if (strip.Mask != null)
            {
                strip.Mask.frontSortingLayerID = isActive 
                    ? SortingLayer.NameToID(spotlightMaskFront) 
                    : strip.OriginalFrontLayerID;

                strip.Mask.backSortingLayerID = isActive 
                    ? SortingLayer.NameToID(spotlightMaskBack) 
                    : strip.OriginalBackLayerID;
            }

            // Animator speed
            foreach (var anim in strip.Animators)
                anim.speed = isActive ? 1f : nonSpotlightAnimSpeed;
        }
    }

    [System.Serializable]
    private class StripData
    {
        public SpriteRenderer[] Renderers;
        public string OriginalLayer;
        public SpriteMask Mask;
        public int OriginalFrontLayerID;
        public int OriginalBackLayerID;
        public Animator[] Animators;

        public StripData(SpriteRenderer[] renderers, string originalLayer, SpriteMask mask, int originalFront, int originalBack, Animator[] animators)
        {
            Renderers = renderers;
            OriginalLayer = originalLayer;
            Mask = mask;
            OriginalFrontLayerID = originalFront;
            OriginalBackLayerID = originalBack;
            Animators = animators;
        }
    }
}
