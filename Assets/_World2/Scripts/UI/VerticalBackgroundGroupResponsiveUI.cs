using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class VerticalBackgroundGroupResponsiveUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _backgroundPrefab;
        [SerializeField] private Sprite _backgroundTopSprite;
        [SerializeField] private Sprite _backgroundBottomSprite;
        [SerializeField] private Sprite _backgroundMiddleSprite;
        [SerializeField] private VerticalLayoutGroupResponsiveUI _verticalLayoutGroupResponsiveUI;

        private readonly List<Image> _bgImageList = new List<Image>();

        private void Awake()
            => _backgroundPrefab.gameObject.SetActive(false);

        private void Start()
        {
            float topHeight = _backgroundTopSprite.rect.size.y;
            float bottomHeight = _backgroundBottomSprite.rect.size.y;
            float middleHeight = _backgroundMiddleSprite.rect.size.y;
            RectTransform rectTransform = (RectTransform)transform;
            float height = rectTransform.rect.height;

            // Calculate how many middle sprites we need
            float remainingHeight = height - topHeight - bottomHeight;
            int middleCount = Mathf.CeilToInt(remainingHeight / middleHeight);
            int totalCount = middleCount + 2; // +2 for top and bottom

            // Create backgrounds
            for (int i = 0; i < totalCount; i++)
            {
                RectTransform bgInstance = Instantiate(_backgroundPrefab, transform);
                Sprite spriteToUse;
                if (i == 0)
                    spriteToUse = _backgroundTopSprite;
                else if (i == totalCount - 1)
                    spriteToUse = _backgroundBottomSprite;
                else
                    spriteToUse = _backgroundMiddleSprite;

                Image bgImage = bgInstance.GetComponent<Image>();
                bgImage.sprite = spriteToUse;
                _bgImageList.Add(bgImage);

                // Positioning
                float yPos;
                if (i == 0)
                    yPos = height / 2 - topHeight / 2;
                else if (i == totalCount - 1)
                    yPos = -height / 2 + bottomHeight / 2;
                else
                    yPos = height / 2 - topHeight - (i - 1) * middleHeight - middleHeight / 2;

                bgInstance.anchoredPosition = new Vector2(0f, yPos);

                // Sizing
                bgInstance.sizeDelta = new Vector2(rectTransform.rect.width, spriteToUse.rect.size.y);

                bgInstance.gameObject.SetActive(true);
            }
        }

        public void RefreshLayout()
            => StartCoroutine(Wait());

        private IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();

            Image bgImage = _bgImageList[0];
            Vector2 size = bgImage.rectTransform.rect.size;

            foreach (Image bgImageInList in _bgImageList)
                bgImageInList.preserveAspect = size.x > 900f;

            Vector2 aspectSize = bgImage.GetPreservedAspectSize();
            Vector2 difference = size - aspectSize;

            float padding = 75f;
            float horizontalPadding = Utils.Halve(difference.x) + padding;

            _verticalLayoutGroupResponsiveUI.SetPadding(
                padding,
                padding,
                horizontalPadding,
                horizontalPadding
            );
        }
    }
}