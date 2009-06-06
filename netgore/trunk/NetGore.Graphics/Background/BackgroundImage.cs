﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single image that resides in the background of the map.
    /// </summary>
    public abstract class BackgroundImage
    {
        /// <summary>
        /// Gets or sets how the background image is aligned to the map.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets the color of the image used when drawing.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the depth of the image relative to other background images, and how fast the
        /// image moves with the camera. A depth of 1.0 will move as fast as the camera, while a depth of
        /// 2.0 will move at half the speed of the camera. Must be greater than or equal to 1.0.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Gets or sets the pixel offset of the image from the Alignment. 
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the ISprite to draw. 
        /// </summary>
        public ISprite Sprite { get; set; }

        /// <summary>
        /// Draws the image to the specified SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the image to.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="mapSize">Size of the map to draw to.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera, Vector2 mapSize)
        {
            if (Sprite == null)
                return;

            Vector2 position = GetPosition(mapSize, camera);
            Sprite.Draw(spriteBatch, position, Color);
        }

        /// <summary>
        /// Draws the image to the specified SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the image to.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="mapSize">Size of the map to draw to.</param>
        /// <param name="spriteSize">Size to draw the sprite.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera, Vector2 mapSize, Vector2 spriteSize)
        {
            if (Sprite == null)
                return;

            Vector2 position = GetPosition(mapSize, camera, spriteSize);
            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, (int)spriteSize.X, (int)spriteSize.Y);
            Sprite.Draw(spriteBatch, rect, Color);
        }

        static Vector2 GetOffsetMultiplier(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0, 0);
                case Alignment.TopRight:
                    return new Vector2(1, 0);
                case Alignment.BottomLeft:
                    return new Vector2(0, 1);
                case Alignment.BottomRight:
                    return new Vector2(1, 1);
                case Alignment.Top:
                    return new Vector2(0.5f, 0);
                case Alignment.Bottom:
                    return new Vector2(0.5f, 1f);
                case Alignment.Left:
                    return new Vector2(0, 0.5f);
                case Alignment.Right:
                    return new Vector2(1, 0.5f);
                case Alignment.Center:
                    return new Vector2(0.5f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException("alignment");
            }
        }

        /// <summary>
        /// Finds the map position of the image using the given <paramref name="camera"/>.
        /// </summary>
        /// <param name="mapSize">Size of the map that this image is on.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="spriteSize">Size of the Sprite that will be drawn.</param>
        /// <returns>The map position of the image using the given <paramref name="camera"/>.</returns>
        public Vector2 GetPosition(Vector2 mapSize, Camera2D camera, Vector2 spriteSize)
        {
            // Can't draw a sprite that has no size...
            if (spriteSize == Vector2.Zero)
                return Vector2.Zero;

            // Get the position from the alignment
            Vector2 alignmentPosition = AlignmentHelper.FindOffset(Alignment, spriteSize, mapSize);

            // Add the custom offset
            Vector2 position = alignmentPosition + Offset;

            // Find the difference between the position and the camera's min position
            Vector2 diff = camera.Min - position;

            // Use the multiplier to align it to the correct part of the camera
            diff += (camera.Size - spriteSize) * GetOffsetMultiplier(Alignment);

            // Compensate for the depth
            diff = diff * ((1 / Depth) - 1);

            // Add the difference to the position
            position -= diff;

            return position;
        }

        /// <summary>
        /// Gets the size of the Sprite source image.
        /// </summary>
        protected Vector2 SpriteSourceSize
        {
            get 
            {
                if (Sprite == null)
                    return Vector2.Zero;

                return new Vector2(Sprite.Source.Width, Sprite.Source.Height); 
            } 
        }

        /// <summary>
        /// Finds the map position of the image using the given <paramref name="camera"/>.
        /// </summary>
        /// <param name="mapSize">Size of the map that this image is on.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <returns>The map position of the image using the given <paramref name="camera"/>.</returns>
        public Vector2 GetPosition(Vector2 mapSize, Camera2D camera)
        {
            return GetPosition(mapSize, camera, SpriteSourceSize);
        }

        /// <summary>
        /// Updates the BackgroundImage.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public virtual void Update(int currentTime)
        {
            // TODO: Sprite.Update(currentTime);
        }
    }
}