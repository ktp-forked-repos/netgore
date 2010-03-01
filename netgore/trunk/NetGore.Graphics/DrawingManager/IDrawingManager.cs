using System;
using System.Linq;

namespace NetGore.Graphics
{
    public interface IDrawingManager
    {
        /// <summary>
        /// Gets the <see cref="ILightManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        ILightManager LightManager { get; }

        /// <summary>
        /// Gets the <see cref="DrawingManagerState"/> describing the current drawing state.
        /// </summary>
        DrawingManagerState State { get; }

        /// <summary>
        /// Begins drawing the graphical user interface, which is not affected by the camera.
        /// </summary>
        /// <returns>The <see cref="ISpriteBatch"/> to use to draw the GUI.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawGUI();

        /// <summary>
        /// Begins drawing of the world.
        /// </summary>
        /// <param name="camera">The camera describing the the current view of the world.</param>
        /// <returns>The <see cref="ISpriteBatch"/> to use to draw the world objects.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawWorld(ICamera2D camera);

        /// <summary>
        /// Ends drawing the graphical user interface.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingGUI"/>.</exception>
        void EndDrawGUI();

        /// <summary>
        /// Ends drawing the world.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingWorld"/>.</exception>
        void EndDrawWorld();

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(int currentTime);
    }
}