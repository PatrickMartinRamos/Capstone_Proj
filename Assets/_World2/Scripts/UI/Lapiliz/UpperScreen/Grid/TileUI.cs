using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    public class TileUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;

        private AzuliuzUI _azuliuzUI;
        private TargetUI _targetUI;
        private Vector2 _gridCoordinates;

        #region Color Management
        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();
        }

        public void SetColor(Color color, float alpha = 0)
        {
            color.a = alpha;
            _visual.color = color;
        }

        public void SetAlpha(float alpha)
        {
            Color color = _visual.color;
            color.a = alpha;
            _visual.color = color;
        }

        public void ResetAlpha()
            => SetAlpha(IsCenterGrid() ? 0.1f : 0f);
        #endregion

        #region Grid Management
        public bool IsReachableFromOrigin(List<TileUI> tileUIList)
        {
            HashSet<Vector2> visited = new HashSet<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();

            Vector2 origin = Vector2.zero; // (0,0)
            queue.Enqueue(origin);
            visited.Add(origin);

            HashSet<Vector2> tileCoords = new HashSet<Vector2>(tileUIList.Select(t => t.GetGridCoordinates()));

            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
                if (current == _gridCoordinates)
                    return true; // reached this tile

                // Check 4 neighbors
                Vector2[] neighbors = {
                    current + Vector2.right,
                    current + Vector2.left,
                    current + Vector2.up,
                    current + Vector2.down
                };

                foreach (var neighbor in neighbors)
                {
                    if (tileCoords.Contains(neighbor) && !visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return false; // not reachable from origin
        }

        public bool IsVisible(Image maskImage, int sampleResolution = 4)
        {
            RectTransform tileRect = (RectTransform)transform;
            Sprite maskSprite = maskImage.sprite;
            Texture2D maskTex = maskSprite.texture;

            Vector3[] corners = new Vector3[4];
            tileRect.GetWorldCorners(corners);

            int visibleCount = 0;
            int totalSamples = sampleResolution * sampleResolution;

            for (int i = 0; i < sampleResolution; i++)
            {
                for (int j = 0; j < sampleResolution; j++)
                {
                    float u = i / (float)(sampleResolution - 1);
                    float v = j / (float)(sampleResolution - 1);

                    Vector3 worldPoint = Vector3.Lerp(
                        Vector3.Lerp(corners[0], corners[1], v), // left to top
                        Vector3.Lerp(corners[3], corners[2], v), // right to top
                        u
                    );

                    Vector2 localPoint = maskImage.rectTransform.InverseTransformPoint(worldPoint);

                    // convert local to sprite UV
                    Rect spriteRect = maskSprite.rect;
                    Vector2 uv = new Vector2(
                        (localPoint.x / maskImage.rectTransform.rect.width + 0.5f) * spriteRect.width + spriteRect.x,
                        (localPoint.y / maskImage.rectTransform.rect.height + 0.5f) * spriteRect.height + spriteRect.y
                    );

                    int x = Mathf.Clamp(Mathf.FloorToInt(uv.x), 0, maskTex.width - 1);
                    int y = Mathf.Clamp(Mathf.FloorToInt(uv.y), 0, maskTex.height - 1);

                    if (maskTex.GetPixel(x, y).a > 0.1f)
                        visibleCount++;
                }
            }

            return visibleCount >= totalSamples * 0.35; // more than 35%
        }
        #endregion

        #region Grid Coordinates Management
        public void SetGridCoordinates(Vector2 gridCoordinates)
            => _gridCoordinates = gridCoordinates;

        public bool IsGridCoordinates(Vector2 gridCoordinates)
            => _gridCoordinates == gridCoordinates;

        public Vector2 GetGridCoordinates()
            => _gridCoordinates;

        public bool IsCenterGrid()
            => IsGridCoordinates(Vector2.zero);
        #endregion

        #region Azuliuz UI Management
        public bool HasAzuliuzUI()
            => _azuliuzUI != null;

        public bool SetAzuliuzUI(AzuliuzUI azuliuzUI)
            => _azuliuzUI = azuliuzUI;

        public AzuliuzUI GetAzuliuzUI()
            => _azuliuzUI;

        public void ClearAzuliuzUI()
            => SetAzuliuzUI(null);
        #endregion

        #region Target UI Management
        public bool HasTargetUI()
            => _targetUI != null;

        public bool SetTargetUI(TargetUI targetUI)
            => _targetUI = targetUI;

        public TargetUI GetTargetUI()
            => _targetUI;

        public void ClearTargetUI()
            => SetTargetUI(null);
        #endregion

    }
}