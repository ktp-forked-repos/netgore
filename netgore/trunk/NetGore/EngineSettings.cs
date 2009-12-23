﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Contains all the engine settings. All values in this class are guaranteed to be immutable and thus may
    /// be cached locally when needed.
    /// </summary>
    public class EngineSettings
    {
        static EngineSettings _instance;
        readonly Vector2 _gravity;
        readonly Vector2 _maxVelocity;

        public EngineSettings(Vector2 gravity, Vector2 maxVelocity)
        {
            _gravity = gravity;
            _maxVelocity = maxVelocity.Abs();
        }

        /// <summary>
        /// Gets the amount of velocity added to an <see cref="Entity"/> every millisecond.
        /// </summary>
        public Vector2 Gravity
        {
            get { return _gravity; }
        }

        /// <summary>
        /// Gets the <see cref="EngineSettings"/> instance. This value will be null until it is set through
        /// <see cref="EngineSettings.Initialize"/>. After being set, this value is guaranteed to contain
        /// the same <see cref="EngineSettings"/> reference.
        /// </summary>
        public static EngineSettings Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the maximum velocity allowed for an <see cref="Entity"/> in any direction. This will always
        /// contain positive values.
        /// </summary>
        public Vector2 MaxVelocity
        {
            get { return _maxVelocity; }
        }

        /// <summary>
        /// Initializes the core engine's settings. This value may only be called once, and must be called as early
        /// as possible before the engine is used.
        /// </summary>
        /// <param name="settings">The engine's settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="MethodAccessException">The settings have already been set.</exception>
        public static void Initialize(EngineSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}