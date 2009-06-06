﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A single simple image that sits in the background of the map.
    /// </summary>
    public class BackgroundLayer : BackgroundImage
    {
        /// <summary>
        /// Gets or sets how the image is drawn on the horizontal axis.
        /// </summary>
        public BackgroundLayerLayout HorizontalLayout { get; set; }

        /// <summary>
        /// Gets or sets how the image is drawn on the vertical axis.
        /// </summary>
        public BackgroundLayerLayout VerticalLayout { get; set; }

        /// <summary>
        /// Draws the image to the specified SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the image to.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="mapSize">Size of the map to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch, Camera2D camera, Vector2 mapSize)
        {
            Vector2 spriteSize = SpriteSourceSize;

            // Adjust the horizontal layout
            switch (HorizontalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.X = GetStretchedSize(camera.X, mapSize.X, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            // Adjust the veritcal layout
            switch (VerticalLayout)
            {
                case BackgroundLayerLayout.Stretched:
                    spriteSize.Y = GetStretchedSize(camera.Y, mapSize.Y, Depth);
                    break;

                case BackgroundLayerLayout.Tiled:
                    throw new NotImplementedException("No support for tiling yet...");
            }

            base.Draw(spriteBatch, camera, mapSize, spriteSize);
        }

        /// <summary>
        /// Gets the size to use for a BackgroundLayer sprite to stretch it across the whole map.
        /// </summary>
        /// <param name="cameraSize">Size of the camera for the given axis.</param>
        /// <param name="targetSize">Target sprite size for the given axis.</param>
        /// <param name="depth">Depth of the BackgroundImage.</param>
        /// <returns>The size to use for a BackgroundLayer sprite to stretch it across the whole map.</returns>
        static protected float GetStretchedSize(float cameraSize, float targetSize, float depth)
        {
            return (targetSize + cameraSize) / depth;
        }
    }
}