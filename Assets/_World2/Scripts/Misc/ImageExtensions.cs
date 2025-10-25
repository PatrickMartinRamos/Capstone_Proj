using System;
using Unity.VisualScripting;
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
            Vector2 textureSize = new(texture.width, texture.height); // px
            Vector4 spriteBorder = image.GetSpriteBorder();
            var actualVisibleSpriteSize = new Vector2(
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
            Vector2 textureSize = new(texture.width, texture.height); // px
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
        public static Vector2 GetActualVisibleSpriteCenter(this Image image)
        {
            Texture texture = image.GetSpriteTexture();
            Vector2 textureSize = new(texture.width, texture.height);
            Vector4 spriteBorder = image.GetSpriteBorder();
            Vector2 actualSpriteSizeInPixels = image.GetActualVisibleSpriteSizeInPixels();

            float originToCenterX =
                Utils.GetDistanceFromOriginToAxisCenter(
                    new Vector2(spriteBorder.x, spriteBorder.z),
                    textureSize.x,
                    actualSpriteSizeInPixels.x);
            float originToCenterY =
                Utils.GetDistanceFromOriginToAxisCenter(
                    new Vector2(spriteBorder.w, spriteBorder.y),
                    textureSize.y,
                    actualSpriteSizeInPixels.y);

            Vector2 originToCenter = new(originToCenterX, originToCenterY);
            return originToCenter;
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
    }
}