using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="GrhData"/> that only contains a single frame. This is the core of all <see cref="GrhData"/>s as it
    /// is also what each frame of an animated <see cref="GrhData"/> contains.
    /// </summary>
    public sealed class StationaryGrhData : GrhData, ITextureAtlasable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _automaticSizeValueKey = "AutomaticSize";
        const string _textureNameValueKey = "Name";
        const string _textureNodeName = "Texture";
        const string _textureSourceValueKey = "Source";

        readonly IContentManager _cm;

        Rectangle _atlasSourceRect;
        bool _automaticSize = false;

        /// <summary>
        /// How many times the texture has failed to load in a row.
        /// </summary>
        byte _failedLoadAttempts = 0;

        bool _isUsingAtlas = false;

        /// <summary>
        /// The current time must be greater than or equal to this value for the texture to allow retrying to reload.
        /// </summary>
        int _nextLoadAttemptTime = int.MinValue;

        Rectangle _sourceRect;
        Image _texture;
        TextureAssetName _textureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/>.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        public StationaryGrhData(IContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _cm = cm;
            AutomaticSize = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="autoGrhData">The <see cref="AutomaticAnimatedGrhData"/>.</param>
        /// <param name="assetName">Name of the asset.</param>
        internal StationaryGrhData(AutomaticAnimatedGrhData autoGrhData, TextureAssetName assetName)
            : base(autoGrhData.Categorization)
        {
            _cm = autoGrhData.ContentManager;
            _textureName = assetName;
            AutomaticSize = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/>.</param>
        /// <param name="cm">The <see cref="IContentManager"/>.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        StationaryGrhData(IValueReader r, IContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _cm = cm;

            var automaticSize = r.ReadBool(_automaticSizeValueKey);
            var textureReader = r.ReadNode(_textureNodeName);
            var textureName = textureReader.ReadTextureAssetName(_textureNameValueKey);
            var textureSource = textureReader.ReadRectangle(_textureSourceValueKey);

            _textureName = textureName;
            _sourceRect = textureSource;
            AutomaticSize = automaticSize;
        }

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s texture has changed.
        /// </summary>
        public event GrhDataChangeTextureHandler TextureChanged;

        /// <summary>
        /// Gets or sets if this GrhData automatically finds the Size by using the whole source texture.
        /// </summary>
        public bool AutomaticSize
        {
            get { return _automaticSize; }
            set
            {
                if (AutomaticSize == value)
                    return;

                _automaticSize = value;

                if (AutomaticSize)
                {
                    _isUsingAtlas = false;
                    _texture = null;
                    ValidateTexture();

                    if (Texture == null)
                    {
                        const string errmsg = "GrhData `{0}` cannot be automatically sized since the texture is null or disposed!";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, this);
                        _sourceRect = new Rectangle(0, 0, 1, 1);
                    }
                    else
                        _sourceRect = new Rectangle(0, 0, (int)Texture.Width, (int)Texture.Height);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IContentManager"/> used to load the content for this <see cref="GrhData"/>.
        /// </summary>
        public IContentManager ContentManager
        {
            get { return _cm; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the frames in an animated <see cref="GrhData"/>, or an
        /// IEnumerable containing a reference to its self if stationary.
        /// </summary>
        public override IEnumerable<StationaryGrhData> Frames
        {
            get { return new StationaryGrhData[] { this }; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of frames in this <see cref="GrhData"/>. If this
        /// is not an animated <see cref="GrhData"/>, this value will always return 0.
        /// </summary>
        public override int FramesCount
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the pixel height for a single frame Grh (SourceRect.Height)
        /// </summary>
        public int Height
        {
            get { return _sourceRect.Height; }
        }

        /// <summary>
        /// Gets the source rectangle of the GrhData on the original texture. This value will remain the same even
        /// when a texture atlas is used.
        /// </summary>
        public Rectangle OriginalSourceRect
        {
            get { return _sourceRect; }
        }

        /// <summary>
        /// Gets the zero-base source pixel position (top-left corner) for a single frame GrhData.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas
                           ? new Vector2(_atlasSourceRect.X, _atlasSourceRect.Y) : new Vector2(_sourceRect.X, _sourceRect.Y);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public override Vector2 Size
        {
            get { return new Vector2(_sourceRect.Width, _sourceRect.Height); }
        }

        /// <summary>
        /// When overridden in the derived class, gets the speed multiplier of the <see cref="GrhData"/> animation where each
        /// frame lasts 1f/Speed milliseconds. For non-animated <see cref="GrhData"/>s, this value will always be 0.
        /// </summary>
        public override float Speed
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the name of the texture used by the GrhData.
        /// </summary>
        public TextureAssetName TextureName
        {
            get { return _textureName; }
        }

        /// <summary>
        /// Gets the pixel width for a single frame GrhData.
        /// </summary>
        public int Width
        {
            get { return _sourceRect.Width; }
        }

        /// <summary>
        /// Gets the pixel X coordinate for a single frame GrhData.
        /// </summary>
        public int X
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect.X : _sourceRect.X;
            }
        }

        /// <summary>
        /// Gets the pixel Y coordinate for a single frame GrhData.
        /// </summary>
        public int Y
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect.Y : _sourceRect.Y;
            }
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        /// <param name="source">A <see cref="Rectangle"/> describing the source area of the texture to
        /// use for this <see cref="GrhData"/>.</param>
        public void ChangeTexture(TextureAssetName newTexture, Rectangle source)
        {
            if (newTexture == null)
                throw new ArgumentNullException("newTexture");

            // Check that the values have changed
            if (source == _sourceRect && TextureName == newTexture)
                return;

            _sourceRect = source;

            // Check that it is actually a different texture
            TextureAssetName oldTextureName = null;
            if (TextureName != newTexture)
                oldTextureName = _textureName;

            // Apply the new texture
            _texture = null;
            _isUsingAtlas = false;
            _textureName = newTexture;

            ValidateTexture();

            if (oldTextureName != null && TextureChanged != null)
                TextureChanged(this, oldTextureName);
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        public void ChangeTexture(TextureAssetName newTexture)
        {
            ChangeTexture(newTexture, GetOriginalSource());
        }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>
        /// A deep copy of this <see cref="GrhData"/>.
        /// </returns>
        protected override GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex)
        {
            StationaryGrhData copy = new StationaryGrhData(ContentManager, newGrhIndex, newCategorization)
            { _textureName = TextureName, _sourceRect = _sourceRect, _automaticSize = _automaticSize };

            return copy;
        }

        /// <summary>
        /// When overridden in the derived class, gets the frame in an animated <see cref="GrhData"/> with the
        /// corresponding index, or null if the index is out of range. If stationary, this will always return
        /// a reference to its self, no matter what the index is.
        /// </summary>
        /// <param name="frameIndex">The index of the frame to get.</param>
        /// <returns>
        /// The frame with the given <paramref name="frameIndex"/>, or null if the <paramref name="frameIndex"/>
        /// is invalid, or a reference to its self if this is not an animated <see cref="GrhData"/>.
        /// </returns>
        public override StationaryGrhData GetFrame(int frameIndex)
        {
            return this;
        }

        /// <summary>
        /// Gets the timeout in milliseconds to wait before trying to load a texture again.
        /// </summary>
        /// <param name="failedLoadAttempts">The number of times the texture has failed to load.</param>
        /// <returns>The timeout in milliseconds to wait before trying to load a texture again.</returns>
        static int GetLoadTextureTimeout(int failedLoadAttempts)
        {
            // If 8 or more failed attempts, it is almost definite this texture isn't loading. However,
            // we will allow it to retry after 30 seconds... just in case it magically starts working again.
            if (failedLoadAttempts >= 8)
                return 1000 * 30;

            // Set the base delay to half a second, so we always wait at least half a second to try again
            int delay = 500;

            // If 3 or more failed load attempts, each failure results in another second being added
            if (failedLoadAttempts >= 3)
                delay += failedLoadAttempts * 1000;

            return delay;
        }

        /// <summary>
        /// Gets the original source rectangle, bypassing any applied atlas
        /// </summary>
        public Rectangle GetOriginalSource()
        {
            return _sourceRect;
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="cm">The <see cref="IContentManager"/> used to load content.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static StationaryGrhData Read(IValueReader r, IContentManager cm)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new StationaryGrhData(r, cm, grhIndex, categorization);
        }

        /// <summary>
        /// Ensures that the texture is properly loaded.
        /// </summary>
        void ValidateTexture()
        {
            // If the texture is not set or is disposed, request a new one
            if (_texture != null)
                return;

            // Check that enough time has elapsed to try and load the texture
            if (_failedLoadAttempts > 0 && _nextLoadAttemptTime > Environment.TickCount)
                return;

            // Try to load the texture
            const string errmsg = "Failed to load texture `{0}` for GrhData `{1}`: {2}";
            try
            {
                _texture = _cm.LoadImage(_textureName, GrhInfo.ContentLevelDecider(this));
            }
            catch (LoadingFailedException ex)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _textureName, this, ex);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _textureName, this, ex);
                Debug.Fail(string.Format(errmsg, _textureName, this, ex));
            }

            // Update the failed loading information if the texture failed to load, or clear it if the texture
            // is valid
            if (_texture != null)
            {
                _failedLoadAttempts = 0;
                _nextLoadAttemptTime = int.MinValue;
            }
            else
            {
                _failedLoadAttempts++;
                _nextLoadAttemptTime = Environment.TickCount + GetLoadTextureTimeout(_failedLoadAttempts);
            }

            // If we were using an atlas, we'll have to remove it because the texture was reloaded
            _isUsingAtlas = false;
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_automaticSizeValueKey, AutomaticSize);
            writer.WriteStartNode(_textureNodeName);
            writer.Write(_textureNameValueKey, TextureName);
            writer.Write(_textureSourceValueKey, GetOriginalSource());
            writer.WriteEndNode(_textureNodeName);
        }

        #region ITextureAtlasable Members

        /// <summary>
        /// Gets the texture source <see cref="Rectangle"/> of the original image.
        /// </summary>
        public Rectangle SourceRect
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect : _sourceRect;
            }
        }

        /// <summary>
        /// Gets the texture for a single frame Grh.
        /// </summary>
        public Image Texture
        {
            get
            {
                ValidateTexture();
                return _texture;
            }
        }

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally.
        /// </summary>
        public void RemoveAtlas()
        {
            if (!_isUsingAtlas)
                return;

            _isUsingAtlas = false;
            _texture = null;
        }

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally only if the given atlas
        /// is the atlas being used. If a different atlas is used, then it will not be removed.
        /// </summary>
        /// <param name="atlas">If the <see cref="ITextureAtlasable"/> is using this atlas, then the atlas
        /// should be removed.</param>
        void ITextureAtlasable.RemoveAtlas(Image atlas)
        {
            if (_isUsingAtlas && _texture == atlas)
                RemoveAtlas();
        }

        /// <summary>
        /// Sets the atlas information.
        /// </summary>
        /// <param name="texture">Texture atlas.</param>
        /// <param name="atlasSourceRect">Source rectangle in the atlas.</param>
        void ITextureAtlasable.SetAtlas(Image texture, Rectangle atlasSourceRect)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            // Set the atlas usage values
            _atlasSourceRect = atlasSourceRect;
            _texture = texture;
            _isUsingAtlas = true;

            // Clear texture loading fail count
            _failedLoadAttempts = 0;
            _nextLoadAttemptTime = int.MinValue;
        }

        #endregion
    }
}