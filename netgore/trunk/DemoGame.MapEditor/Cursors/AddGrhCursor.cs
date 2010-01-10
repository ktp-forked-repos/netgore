using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    sealed class AddGrhCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;
        readonly MenuItem _mnuForeground;

        public MenuItem SnapToGridMenuItem { get { return _mnuSnapToGrid; } }

        public bool AddToForeground { get { return _mnuForeground.Checked; } }

        public bool SnapToGrid { get { return SnapToGridMenuItem.Checked; } }

        void Menu_SnapToGrid_Click(object sender, EventArgs e)
        {
            _mnuSnapToGrid.Checked = !_mnuSnapToGrid.Checked;
        }

        void Menu_Foreground_Click(object sender, EventArgs e)
        {
            _mnuForeground.Checked = !_mnuForeground.Checked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddGrhCursor"/> class.
        /// </summary>
        public AddGrhCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _mnuForeground = new MenuItem("Foreground", Menu_Foreground_Click) { Checked = false };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid, _mnuForeground });
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="ContextMenu"/> used by this cursor
        /// to display additional functions and settings.
        /// </summary>
        /// <param name="cursorManager">The cursor manager.</param>
        /// <returns>
        /// The <see cref="ContextMenu"/> used by this cursor to display additional functions and settings,
        /// or null for no <see cref="ContextMenu"/>.
        /// </returns>
        public override ContextMenu GetContextMenu(MapEditorCursorManager<ScreenForm> cursorManager)
        {
            return _contextMenu;
        }

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_grhsadd; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Grh"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 20; }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            if (_mnuSnapToGrid.Checked)
                MouseUp(screen, e);
        }

        /// <summary>
        /// Color of the Grh preview when placing new Grhs.
        /// </summary>
        static readonly Microsoft.Xna.Framework.Graphics.Color _drawPreviewColor = new Microsoft.Xna.Framework.Graphics.Color(255, 255, 255, 150);

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void DrawInterface(ScreenForm screen)
        {
            if (screen.SelectedGrh.GrhData != null)
            {
                Vector2 drawPos;
                if (_mnuSnapToGrid.Checked)
                    drawPos = screen.Grid.AlignDown(screen.CursorPos);
                else
                    drawPos = screen.CursorPos;

                // If we fail to draw the selected Grh, just ignore it
                try
                {
                    screen.SelectedGrh.Draw(screen.SpriteBatch, drawPos, _drawPreviewColor);
                }
                catch (Exception)
                {
                }
            }

            base.DrawInterface(screen);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            // On left-click place the Grh on the map
            if (e.Button == MouseButtons.Left)
            {
                // Check for a valid MapGrh
                if (screen.SelectedGrh.GrhData == null)
                    return;

                // Find the position the MapGrh will be created at
                Vector2 drawPos;
                if (_mnuSnapToGrid.Checked)
                    drawPos = screen.Grid.AlignDown(cursorPos);
                else
                    drawPos = cursorPos;

                // Check if a MapGrh of the same type already exists at the location
                foreach (MapGrh grh in screen.Map.MapGrhs)
                {
                    if (grh.Position == drawPos && grh.Grh.GrhData.GrhIndex == screen.SelectedGrh.GrhData.GrhIndex)
                        return;
                }

                // Add the MapGrh to the map
                Grh g = new Grh(screen.SelectedGrh.GrhData, AnimType.Loop, screen.GetTime());
                screen.Map.AddMapGrh(new MapGrh(g, drawPos, _mnuForeground.Checked));
            }
            else if (e.Button == MouseButtons.Right)
            {
                // On right-click delete any Grhs under the cursor
                while (true)
                {
                    MapGrh mapGrh = screen.Map.Spatial.GetEntity<MapGrh>(cursorPos);
                    if (mapGrh == null)
                        break;

                    screen.Map.RemoveMapGrh(mapGrh);
                }
            }
        }
    }
}