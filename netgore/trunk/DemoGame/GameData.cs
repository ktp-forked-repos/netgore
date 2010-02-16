using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains static data for the game.
    /// </summary>
    public static class GameData
    {
        /// <summary>
        /// If a User is allowed to move while they have a chat dialog open with a NPC.
        /// </summary>
        public const bool AllowMovementWhileChattingToNPC = false;

        /// <summary>
        /// The velocity to assume all character movement animations are made at. That is, if the character is moving
        /// with a velocity equal to this value, the animation will update at the usual speed. If it is twice as much
        /// as this value, the character's animation will update twice as fast. This is to make the rate a character
        /// moves proportionate to the rate their animation is moving.
        /// </summary>
        public const float AnimationSpeedModifier = 0.13f;

        /// <summary>
        /// The maximum distance a target can be from a character for them to be targeted.
        /// </summary>
        public const float MaxTargetDistance = 500f;

        /// <summary>
        /// The IP address to use by default when creating accounts when no IP can be specified, such as if the account
        /// is created from the console.
        /// </summary>
        public const uint DefaultCreateAccountIP = 0;

        /// <summary>
        /// Maximum number of characters allowed in a single account.
        /// </summary>
        public const byte MaxCharactersPerAccount = 10;

        /// <summary>
        /// Maximum length of a Say packet's string from the client to the server.
        /// </summary>
        public const int MaxClientSayLength = 250;

        /// <summary>
        /// The maximum size (number of different item sets) of the Inventory. Any slot greater than or equal to
        /// the MaxInventorySize is considered invalid.
        /// </summary>
        public const int MaxInventorySize = 6 * 6;

        /// <summary>
        /// The maximum accounts that can be created for a single IP address over a given period of time. The period
        /// of time is defined by the query itself (CountRecentlyCreatedAccounts).
        /// </summary>
        public const int MaxRecentlyCreatedAccounts = 3;

        /// <summary>
        /// Maximum length of each parameter string in the server's SendMessage.
        /// </summary>
        public const int MaxServerMessageParameterLength = 250;

        /// <summary>
        /// Maximum length of a Say packet's string from the server to the client.
        /// </summary>
        public const int MaxServerSayLength = 500;

        /// <summary>
        /// Maximum length of the Name string used by the server's Say messages.
        /// </summary>
        public const int MaxServerSayNameLength = 60;

        /// <summary>
        /// The rules for the account email addresses.
        /// </summary>
        public static readonly StringRules AccountEmail = new StringRules(3, 30,
                                                                          CharType.Alpha | CharType.Numeric | CharType.Punctuation);

        /// <summary>
        /// The rules for the account names.
        /// </summary>
        public static readonly StringRules AccountName = new StringRules(3, 30, CharType.Alpha | CharType.Numeric);

        /// <summary>
        /// The rules for the account passwords.
        /// </summary>
        public static readonly StringRules AccountPassword = new StringRules(3, 30,
                                                                             CharType.Alpha | CharType.Numeric |
                                                                             CharType.Punctuation);

        /// <summary>
        /// The rules for the character names.
        /// </summary>
        public static readonly StringRules CharacterName = new StringRules(3, 15, CharType.Alpha);

        /// <summary>
        /// Size of the screen display.
        /// </summary>
        public static Vector2 ScreenSize = new Vector2(1024, 768);

        /// <summary>
        /// Gets the maximum delta time between draws for any kind of drawable component. If the delta time between
        /// draw calls on the component exceeds this value, the delta time should then be reduced to be equal to this value.
        /// </summary>
        public static int MaxDrawDeltaTime
        {
            get { return 100; }
        }

        /// <summary>
        /// Gets the IP address of the server.
        /// </summary>
        public static string ServerIP
        {
            get { return "127.0.0.1"; }
        }

        /// <summary>
        /// Gets the port used by the server for handling pings.
        /// </summary>
        public static int ServerPingPort
        {
            get { return 44446; }
        }

        /// <summary>
        /// Gets the port used by the server for TCP connections.
        /// </summary>
        public static int ServerTCPPort
        {
            get { return 44445; }
        }

        /// <summary>
        /// Gets the number of milliseconds between each World update step. This only applies to the synchronized
        /// physics, not client-side visuals.
        /// </summary>
        public static int WorldPhysicsUpdateRate
        {
            get { return 20; }
        }

        /// <summary>
        /// Gets the default amount of money a Character will pay for buying the given <paramref name="item"/> from
        /// a shop.
        /// </summary>
        /// <param name="item">The item to purchase.</param>
        /// <returns>the default amount of money a Character will pay for buying the given <paramref name="item"/>
        /// from a shop.</returns>
        public static int GetItemBuyValue(ItemEntityBase item)
        {
            return item.Value;
        }

        /// <summary>
        /// Gets the default amount of money a Character will get for selling the given <paramref name="item"/> to
        /// a shop.
        /// </summary>
        /// <param name="item">The item to sell.</param>
        /// <returns>the default amount of money a Character will get for selling the given <paramref name="item"/>
        /// to a shop.</returns>
        public static int GetItemSellValue(ItemEntityBase item)
        {
            return Math.Max(item.Value / 2, 1);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the valid region that items can be picked up at from
        /// the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> doing the picking-up.</param>
        /// <returns>A <see cref="Rectangle"/> that represents the valid region that items can be picked up at from
        /// the given <paramref name="spatial"/>.</returns>
        public static Rectangle GetPickupRegion(ISpatial spatial)
        {
            const int padding = 70;
            var r = spatial.ToRectangle();
            return new Rectangle(r.X - padding, r.Y - padding, r.Width + (padding * 2), r.Height + (padding * 2));
        }

        /// <summary>
        /// Gets if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to shop.
        /// </summary>
        /// <param name="shopper">The Entity doing the shopping.</param>
        /// <param name="shopOwner">The Entity that owns the shop.</param>
        /// <returns>True if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to
        /// shop; otherwise false.</returns>
        public static bool IsValidDistanceToShop(Entity shopper, Entity shopOwner)
        {
            return shopper.Intersects(shopOwner);
        }

        /// <summary>
        /// Checks if an <see cref="ISpatial"/> is close enough to another <see cref="ISpatial"/> to pick it up.
        /// </summary>
        /// <param name="grabber">The <see cref="ISpatial"/> doing the picking up.</param>
        /// <param name="toGrab">The <see cref="ISpatial"/> being picked up.</param>
        /// <returns>True if the <paramref name="grabber"/> is close enough to the <paramref name="toGrab"/>
        /// to pick it up; otherwise false.</returns>
        public static bool IsValidPickupDistance(ISpatial grabber, ISpatial toGrab)
        {
            var region = GetPickupRegion(grabber);
            return toGrab.Intersects(region);
        }

        /// <summary>
        /// Gets the experience required for a given level.
        /// </summary>
        /// <param name="x">Level to check (current level).</param>
        /// <returns>Experience required for the given level.</returns>
        public static int LevelCost(int x)
        {
            return x * 30;
        }

        /// <summary>
        /// Converts the integer-based movement speed to a real velocity value.
        /// </summary>
        /// <param name="movementSpeed">The integer-based movement speed.</param>
        /// <returns>The real velocity value for the given integer-based movement speed.</returns>
        public static float MovementSpeedToVelocity(int movementSpeed)
        {
            return movementSpeed / 10000.0f;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes all of the potential area that
        /// a ranged attack can reach.
        /// </summary>
        /// <param name="c">The <see cref="CharacterEntity"/> that is attacking.</param>
        /// <param name="range">The range of the attack.</param>
        /// <returns>A <see cref="Rectangle"/> that describes all of the potential area that
        /// a ranged attack can reach.</returns>
        public static Rectangle GetRangedAttackArea(CharacterEntity c, ushort range)
        {
            var vrange = new Vector2(range);
            var min = c.Position - vrange;
            var max = c.Max + vrange;
            return new Rectangle((int)min.X, (int)min.Y, (int)max.X, (int)max.Y);
        }
        
        /// <summary>
        /// Gets a <see cref="Rectangle"/> containing the hit area for a melee attack.
        /// </summary>
        /// <param name="c">The <see cref="CharacterEntity"/> that is attacking.</param>
        /// <param name="range">The range of the attack.</param>
        /// <returns>The <see cref="Rectangle"/> that describes the hit area for a melee attack.</returns>
        public static Rectangle GetMeleeAttackArea(CharacterEntity c, ushort range)
        {
            var pos = c.Position;
            var size= c.Size;

            if (c.Heading == Direction.East)
            {
                return new Rectangle((int)pos.X - range, (int)pos.Y, (int)(range + size.X), (int)size.Y);
            }
            else
            {
                return new Rectangle((int)pos.X, (int)pos.Y, (int)(pos.X + size.X + range), (int)size.Y);
            }
        }

        /// <summary>
        /// Gets the experience required for a given stat level.
        /// </summary>
        /// <param name="x">Stat level to check (current stat level).</param>
        /// <returns>Experience required for the given stat level.</returns>
        public static int StatCost(int x)
        {
            return (x / 10) + 1;
        }

        /// <summary>
        /// Converts a real velocity value to the integer-based movement speed.
        /// </summary>
        /// <param name="velocity">The real velocity value.</param>
        /// <returns>The integer-based movement speed for the given real velocity value.</returns>
        public static int VelocityToMovementSpeed(float velocity)
        {
            return (int)(velocity * 10000.0f);
        }
    }
}