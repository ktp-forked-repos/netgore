using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages the GUI
    /// </summary>
    public class GUIManager : GUIManagerBase
    {
        /// <summary>
        /// Static instance of the System.Blank GrhData used as the default blank sprite
        /// </summary>
        static Grh _blankGrh = null;

        /// <summary>
        /// Gets the blank Grh, ensuring it is loaded
        /// </summary>
        static Grh BlankGrh
        {
            get
            {
                if (_blankGrh == null)
                {
                    GrhData gd = GrhInfo.GetData("System", "Blank");
                    if (gd == null)
                        throw new Exception("Failed to load the System.Blank GrhData.");
                    _blankGrh = new Grh(gd);
                }
                return _blankGrh;
            }
        }

        /// <summary>
        /// GUIManager constructor
        /// </summary>
        /// <param name="font">Default SpriteFont to use for controls added to this GUIManager</param>
        public GUIManager(SpriteFont font) : base(font, BlankGrh)
        {
            LoadSettings(Skin.Current);
            Skin.OnChange += Skin_OnChange;
        }

        /// <summary>
        /// Provides for easier creation of a border
        /// </summary>
        /// <param name="dic">Dictionary containing the border information</param>
        /// <returns>ControlBorder created from the dictionary information</returns>
        static ControlBorder CreateBorder(IDictionary<string, GrhData> dic)
        {
            ISprite bg = new Grh(dic["Background"]);
            ISprite l = new Grh(dic["Left"]);
            ISprite r = new Grh(dic["Right"]);
            ISprite b = new Grh(dic["Bottom"]);
            ISprite bl = new Grh(dic["BottomLeft"]);
            ISprite br = new Grh(dic["BottomRight"]);
            ISprite tl = new Grh(dic["TopLeft"]);
            ISprite t = new Grh(dic["Top"]);
            ISprite tr = new Grh(dic["TopRight"]);
            return new ControlBorder(tl, t, tr, r, br, b, bl, l, bg);
        }

        /// <summary>
        /// Loads all of the default settings for the controls of this GUIManager
        /// </summary>
        /// <param name="skin">Name of the skin to load</param>
        public void LoadSettings(string skin)
        {
            string root = "GUI." + skin + ".Controls.";

            // NOTE: Revert to "Default" skin if something isn't found (use Skin.GetSkinGrh or whatever)
            
            ControlBorder cbForm = CreateBorder(GrhInfo.GetData(root + "Form"));
            ControlBorder cbTextBox = CreateBorder(GrhInfo.GetData(root + "TextBox"));
            ControlBorder cbButton = CreateBorder(GrhInfo.GetData(root + "Button"));
            ControlBorder cbButtonPressed = CreateBorder(GrhInfo.GetData(root + "Button.Pressed"));
            ControlBorder cbButtonOver = CreateBorder(GrhInfo.GetData(root + "Button.MouseOver"));

            var dic = GrhInfo.GetData(root + "CheckBox");
            ISprite ut = new Grh(dic["Unticked"]);
            ISprite utOver = new Grh(dic["UntickedMouseOver"]);
            ISprite utPressed = new Grh(dic["UntickedPressed"]);
            ISprite t = new Grh(dic["Ticked"]);
            ISprite tOver = new Grh(dic["TickedMouseOver"]);
            ISprite tPressed = new Grh(dic["TickedPressed"]);

            ButtonSettings = new ButtonSettings(cbButton, cbButtonOver, cbButtonPressed);
            CheckBoxSettings = new CheckBoxSettings(null, t, tOver, tPressed, ut, utOver, utPressed);
            FormSettings = new FormSettings(Color.White, cbForm);
            TextBoxSettings = new TextBoxSettings(cbTextBox);
        }

        /// <summary>
        /// Loads the new skin upon change.
        /// </summary>
        void Skin_OnChange(string newSkin, string oldSkin)
        {
            LoadSettings(newSkin);
        }
    }
}