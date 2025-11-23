using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Gets the scaled sprite size of the visible (non-transparent) portion of a UI Image, in UI units.
        /// </summary>
        /// <param name="image">The Image component to evaluate.</param>
        /// <returns>The scaled visible sprite size in UI units.</returns>
        public static Vector2 GetScaledVisibleSpriteSizeInUIUnits(this Image image)
        {
            Sprite sprite = image.sprite;
            if (sprite == null)
            {
                Debug.LogError("No sprite assigned.");
                return image.rectTransform.rect.size;
            }

            float ppu = sprite.pixelsPerUnit;

            Vector2 actualSpriteSizeInPixels = image.GetActualVisibleSpriteSizeInPixels();
            Vector2 actualSpriteSizeInUIUnits = Utils.ConvertPixelsToUIUnits(actualSpriteSizeInPixels, ppu);

            Vector2 displayedSpriteSizeInUIUnits = image.GetDisplayedSpriteSizeInUIUnits();

            Vector2 spriteRectInUIUnits = Utils.ConvertPixelsToUIUnits(sprite.rect.size, ppu);
            var scale = new Vector2(
                displayedSpriteSizeInUIUnits.x / spriteRectInUIUnits.x,
                displayedSpriteSizeInUIUnits.y / spriteRectInUIUnits.y
            );

            var scaledSpriteSize = new Vector2(
                actualSpriteSizeInUIUnits.x * scale.x,
                actualSpriteSizeInUIUnits.y * scale.y
            );

            return scaledSpriteSize;
        }

        /// <summary>
        /// Gets the actual sprite size of the visible (non-transparent) portion of a UI Image, in Pixels.
        /// </summary>
        /// <param name="image">The Image component to evaluate.</param>
        /// <returns>The actual visible sprite size in Pixels.</returns>
        public static Vector2 GetActualVisibleSpriteSizeInPixels(this Image image)
        {
            Texture texture = image.GetSpriteTexture();
            Vector2 textureSize = new Vector2(texture.width, texture.height); // px
            Vector4 spriteBorder = image.GetSpriteBorder();
            Vector2 actualVisibleSpriteSize = new Vector2(
                Utils.MultiSubtract(textureSize.x, spriteBorder.x, spriteBorder.z),
                Utils.MultiSubtract(textureSize.y, spriteBorder.y, spriteBorder.w));

            return actualVisibleSpriteSize;
        }

        /// <summary>
        /// Gets the displayed size of a sprite, in UI Units
        /// </summary>
        /// <param name="image"></param>
        /// <returns>The display size of a sprite in UI Units.</returns>
        public static Vector2 GetDisplayedSpriteSizeInUIUnits(this Image image)
        {
            Texture texture = image.GetSpriteTexture();
            Vector2 textureSize = new Vector2(texture.width, texture.height); // px
            Vector2 rectSize = image.rectTransform.rect.size; // ui

            if (image.preserveAspect)
            {
                float textureAspect = textureSize.x / textureSize.y; // px / px = unitless
                float rectAspect = rectSize.x / rectSize.y; // ui / ui = unitless

                if (textureAspect > rectAspect)
                {
                    float height = rectSize.x / textureAspect; // ui / unitless = ui
                    return new Vector2(rectSize.x, height);
                }
                else
                {
                    float width = rectSize.y * textureAspect; // ui / unitless = ui
                    return new Vector2(width, rectSize.y);
                }
            }

            return rectSize;
        }

        /// <summary>
        /// Gets the center of the actual visible sprite (non-transparent) portion of a UI Image.
        /// </summary>
        /// <param name="image">The Image component to evaluate.</param>
        /// <returns>The center of actual visible sprite.</returns>
        public static Vector2 GetScaledVisibleSpriteCenter(this Image image)
        {
            Texture texture = image.GetSpriteTexture();
            Vector2 textureSize = new(texture.width, texture.height);
            Vector4 spriteBorder = image.GetSpriteBorder();
            RectTransform rectTransform = image.rectTransform;

            // 1️⃣ Compute the center position in texture space (in pixels)
            float originToCenterX =
                Utils.GetDistanceFromOriginToAxisCenter(
                    new Vector2(spriteBorder.x, spriteBorder.z),
                    textureSize.x);
            float originToCenterY =
                Utils.GetDistanceFromOriginToAxisCenter(
                    new Vector2(spriteBorder.y, spriteBorder.w),
                    textureSize.y);

            Vector2 textureCenter = new(originToCenterX, originToCenterY);

            // 2️⃣ Normalize relative to texture size
            Vector2 normalizedCenter = new Vector2(
                textureCenter.x / textureSize.x,
                textureCenter.y / textureSize.y
            );

            // 3️⃣ Scale by the actual visible UI rect size
            Vector2 uiRectSize = rectTransform.rect.size;
            Vector2 uiCenter = new Vector2(
                normalizedCenter.x * uiRectSize.x,
                normalizedCenter.y * uiRectSize.y
            );

            return uiCenter;
        }

        public static Texture GetSpriteTexture(this Image image)
        {
            // Sprite is just a portion of a texture
            Sprite sprite = image.sprite;

            if (sprite == null)
            {
                Debug.LogError("No sprite assigned.");
                return image.mainTexture;
            }

            return sprite.texture;
        }

        public static Vector4 GetSpriteBorder(this Image image)
        {
            // Sprite is just a portion of a texture
            Sprite sprite = image.sprite;

            if (sprite == null)
            {
                Debug.LogError("No sprite assigned.");
                return image.rectTransform.rect.size;
            }

            return sprite.border;
        }

        public static Rect GetVisibleRect(this Image image)
        {
            Vector2 center = image.GetScaledVisibleSpriteCenter();
            Vector2 size = image.GetScaledVisibleSpriteSizeInUIUnits();

            return new Rect(center, size);
        }

        public static void ConfigureImageFromFullToVisibleSprite(this Image image, Sprite full, Sprite visible)
        {
            image.ConfigureImageAsFullSprite(full);
            image.ConfigureImageAsVisibleSprite(visible);
        }

        public static void ConfigureImageAsFullSprite(this Image image, Sprite sprite)
        {
            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.one;
            Vector2 anchoredPosition = Vector2.zero;
            Vector2 sizeDelta = Vector2.zero;

            image.ConfigureImageLayoutAndSprite(
                anchorMin,
                anchorMax,
                anchoredPosition,
                sizeDelta,
                sprite
            );
        }

        public static void ConfigureImageAsVisibleSprite(this Image image, Sprite sprite)
        {
            Vector2 anchorMin = new Vector2(0.5f, 0.5f);
            Vector2 anchorMax = new Vector2(0.5f, 0.5f);
            Vector2 anchoredPosition = image.GetScaledVisibleSpriteCenter();
            Vector2 sizeDelta = image.GetScaledVisibleSpriteSizeInUIUnits();

            image.ConfigureImageLayoutAndSprite(
                anchorMin,
                anchorMax,
                anchoredPosition,
                sizeDelta,
                sprite
            );
        }

        public static void ConfigureImageLayoutAndSprite(
            this Image image,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Sprite sprite)
        {
            RectTransform imageRectTransform = image.rectTransform;
            imageRectTransform.anchorMin = anchorMin;
            imageRectTransform.anchorMax = anchorMax;
            imageRectTransform.anchoredPosition = anchoredPosition;
            imageRectTransform.sizeDelta = sizeDelta;
            image.sprite = sprite;
        }
    }
}