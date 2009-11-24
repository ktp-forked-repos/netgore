﻿using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;
using NUnit.Framework;

namespace NetGore.Tests.Graphics.ParticleEngine
{
    [TestFixture]
    public class ParticleModifierTests
    {
        [Test]
        public void ConstructorTest()
        {
            new TestModifier(true, true);
            new TestModifier(true, false);
            new TestModifier(false, true);

            Assert.Throws<ArgumentException>(() => new TestModifier(false, false));
        }

        class TestModifier : ParticleModifierBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParticleModifierBase"/> class.
            /// </summary>
            /// <param name="processOnRelease">If <see cref="Particle"/>s will be processed after being released.</param>
            /// <param name="processOnUpdate">If <see cref="Particle"/>s will be processed after being updated.</param>
            public TestModifier(bool processOnRelease, bool processOnUpdate) : base(processOnRelease, processOnUpdate)
            {
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
            /// it is released. Only valid if <see cref="ParticleModifierBase.ProcessOnRelease"/> is set.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
            /// came from.</param>
            /// <param name="particle">The <see cref="Particle"/> to process.</param>
            protected override void HandleProcessReleased(ParticleEmitter emitter, Particle particle)
            {
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
            /// it is updated. Only valid if <see cref="ParticleModifierBase.ProcessOnUpdate"/> is set.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
            /// came from.</param>
            /// <param name="particle">The <see cref="Particle"/> to process.</param>
            /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
            /// was last updated.</param>
            protected override void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
            {
            }
        }
    }
}