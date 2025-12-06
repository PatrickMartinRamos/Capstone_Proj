using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class HorizontalBackgroundGroupResponsiveUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _backgroundPrefab;
        [SerializeField] private Sprite _backgroundLeftSprite;
        [SerializeField] private Sprite _backgroundRightSprite;
        [SerializeField] private Sprite _backgroundMiddleSprite;
        // [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

        private readonly List<Image> _bgImageList = new List<Image>();

        private void Awake()
            => _backgroundPrefab.gameObject.SetActive(false);

        private void Start()
        {
            float leftWidth = _backgroundLeftSprite.rect.size.x;
            float rightWidth = _backgroundRightSprite.rect.size.x;
            float middleWidth = _backgroundMiddleSprite.rect.size.x;
            RectTransform rectTransform = (RectTransform)transform;
            float width = rectTransform.rect.width;

            // Calculate how many middle sprites we need
            float remainingWidth = width - leftWidth - rightWidth;
            int middleCount = Mathf.CeilToInt(remainingWidth / middleWidth);
            int totalCount = middleCount + 2; // +2 for left and right

            // Create backgrounds
            for (int i = 0; i < totalCount; i++)
            {
                RectTransform bgInstance = Instantiate(_backgroundPrefab, transform);
                Sprite spriteToUse;
                if (i == 0)
                    spriteToUse = _backgroundLeftSprite;
                else if (i == totalCount - 1)
                    spriteToUse = _backgroundRightSprite;
                else
                    spriteToUse = _backgroundMiddleSprite;

                Image bgImage = bgInstance.GetComponent<Image>();
                bgImage.sprite = spriteToUse;
                _bgImageList.Add(bgImage);

                // Positioning
                float xPos;
                if (i == 0)
                    xPos = width / 2 - leftWidth / 2;
                else if (i == totalCount - 1)
                    xPos = -width / 2 + rightWidth / 2;
                else
                    xPos = width / 2 - leftWidth - (i - 1) * middleWidth - middleWidth / 2;

                bgInstance.anchoredPosition = new Vector2(0f, xPos);

                // Sizing
                bgInstance.sizeDelta = new Vector2(spriteToUse.rect.size.x, rectTransform.rect.height);

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

            // Vector2 aspectSize = bgImage.GetPreservedAspectSize();
            // Vector2 difference = size - aspectSize;

            // float padding = 75f;
            // float horizontalPadding = Utils.Halve(difference.x) + padding;

            // _horizontalLayoutGroup.SetPadding(
            //     padding,
            //     padding,
            //     horizontalPadding,
            //     horizontalPadding
            // );
        }
    }
}