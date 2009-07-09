using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// A non-player character
    /// </summary>
    public class NPC : Character, IRespawnable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly NPCEquipped _equipped;
        readonly NPCInventory _inventory;

        readonly NPCStats _stats;
        AIBase _ai;

        ushort _giveCash;
        ushort _giveExp;

        ushort _respawnSecs;

        /// <summary>
        /// The game time at which the NPC will respawn.
        /// </summary>
        int _respawnTime = 0;

        /// <summary>
        /// Gets the NPC's AI. Can be null.
        /// </summary>
        public AIBase AI
        {
            get { return _ai; }
        }

        public override CharacterEquipped Equipped
        {
            get { return _equipped; }
        }

        public ushort GiveCash
        {
            get { return _giveCash; }
            protected set { _giveCash = value; }
        }

        public ushort GiveExp
        {
            get { return _giveExp; }
            protected set { _giveExp = value; }
        }

        /// <summary>
        /// Gets the Character's Inventory.
        /// </summary>
        public override CharacterInventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets or sets the Map the NPC will respawn on after dying.
        /// </summary>
        public Map RespawnMap { get; set; }

        /// <summary>
        /// Gets or sets (protected) the amount of time it takes (in milliseconds) for the NPC to respawn.
        /// </summary>
        public ushort RespawnSecs
        {
            get { return _respawnSecs; }
            protected set { _respawnSecs = value; }
        }

        /// <summary>
        /// Gets the CharacterStatsBase used for this Character's stats.
        /// </summary>
        public override CharacterStatsBase Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// Gets if this NPC will respawn after dieing.
        /// </summary>
// ReSharper disable MemberCanBeMadeStatic.Global
        public bool WillRespawn // ReSharper restore MemberCanBeMadeStatic.Global
        {
            get { return RespawnMap != null; }
        }

        [Obsolete("Do not use this empty constructor on the Server!")]
        public NPC()
        {
        }

        public NPC(World parent, CharacterID characterID) : base(parent, true)
        {
            // HACK: This whole constructor is uber hax
            if (parent == null)
                throw new ArgumentNullException("parent");

            // Set up the inventory
            _inventory = new NPCInventory(this);
            _equipped = new NPCEquipped(this);
            _stats = new NPCStats(this);

            Alliance = parent.Server.AllianceManager["monster"];

            Load(characterID);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created persistent NPC `{0}` from CharacterID `{1}`.", this, characterID);
        }

        /// <summary>
        /// NPC constructor
        /// </summary>
        /// <param name="parent">World that the NPC belongs to.</param>
        /// <param name="template">NPCTemplate used to create the NPC.</param>
        public NPC(World parent, CharacterTemplate template) : base(parent, false)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (template == null)
                throw new ArgumentNullException("template");

            // Set up the inventory
            _inventory = new NPCInventory(this);
            _equipped = new NPCEquipped(this);

            // Set up the NPC
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Name = template.Name;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
            Alliance = template.Alliance;
            BodyInfo = GameData.Body(template.BodyIndex);
            TemplateID = template.ID;
            CB = new CollisionBox(BodyInfo.Width, BodyInfo.Height);

            // Create the AI
            if (!string.IsNullOrEmpty(template.AIName))
                SetAI(template.AIName);

            // Create and copy over the stats
            _stats = new NPCStats(this);
            _stats.CopyStatValuesFrom(template.Stats, false);

            _stats[StatType.HP] = _stats[StatType.MaxHP];
            _stats[StatType.MP] = _stats[StatType.MaxMP];

            // Set the rest of the template stuff
            _respawnSecs = template.RespawnSecs;
            _giveExp = template.GiveExp;
            _giveCash = template.GiveCash;

            LoadSpawnItems();

            // Done loading
            SetAsLoaded();

            if (log.IsInfoEnabled)
                log.InfoFormat("Created NPC instance from template `{0}`.", template);
        }

        /// <summary>
        /// Gives an item to the Character.
        /// </summary>
        /// <param name="item">Item to give to the character.</param>
        /// <returns>The remainder of the item that failed to be added to the inventory, or null if all of the
        /// item was added.</returns>
        public override ItemEntity GiveItem(ItemEntity item)
        {
            return _inventory.Add(item);
        }

        /// <summary>
        /// When overridden in the derived class, implements the Character being killed. This 
        /// doesn't actually care how the Character was killed, it just takes the appropriate
        /// actions to kill them.
        /// </summary>
        public override void Kill()
        {
            base.Kill();

            if (!IsAlive)
            {
                const string errmsg = "Attempted to kill dead NPC `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }
            if (Map == null)
            {
                const string errmsg = "Attempted to kill NPC `{0}` with a null map.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            // Drop items
            foreach (var item in Inventory)
            {
                Inventory.RemoveAt(item.Key);
                DropItem(item.Value);
            }

            foreach (var item in Equipped)
            {
                Equipped.RemoveAt(item.Key);
            }

            // Check to respawn
            if (WillRespawn)
            {
                // Start the respawn sequence
                IsAlive = false;
                _respawnTime = GetTime() + (RespawnSecs * 1000);

                LoadSpawnItems();

                ChangeMap(null);
                RespawnMap.AddToRespawn(this);
            }
            else
            {
                // No respawning, so just dispose
                DelayedDispose();
            }
        }

        /// <summary>
        /// Reloads the Inventory and Equipment items the NPC spawns with.
        /// </summary>
        void LoadSpawnItems()
        {
            // All items remaining in the inventory or equipment should NOT be referenced!
            // Items that were dropped should have been removed when dropping
            _inventory.RemoveAll(true);
            _equipped.RemoveAll(true);

            // Grab the respawn items from the template
            var templateID = TemplateID;
            if (!templateID.HasValue)
                return;

            CharacterTemplate template = World.NPCTemplates[templateID.Value];
            if (template == null)
                return;

            var spawnInventory = template.Inventory;
            var spawnEquipment = template.Equipment;

            // Create the items
            if (spawnInventory != null)
            {
                foreach (CharacterTemplateInventoryItem inventoryItem in spawnInventory)
                {
                    ItemEntity item = inventoryItem.CreateInstance();
                    if (item == null)
                        continue;

                    ItemEntity extraItems = Inventory.Add(item);
                    if (extraItems != null)
                        extraItems.Dispose();
                }
            }
            
            
            if (spawnEquipment != null)
            {
                foreach (CharacterTemplateEquipmentItem equippedItem in spawnEquipment)
                {
                    ItemEntity item = equippedItem.CreateInstance();
                    if (item == null)
                        continue;
                    
                    if (!Equipped.Equip(item))
                        item.Dispose();
                }
            }
        }

        /// <summary>
        /// Sets the NPC's AI.
        /// </summary>
        /// <param name="aiName">Name of the AI to change to. Use a null or empty string to set the AI to null.</param>
        public void SetAI(string aiName)
        {
            if (string.IsNullOrEmpty(aiName))
                _ai = null;
            else
            {
                AIBase newAI;
                try
                {
                    newAI = AIFactory.Create(aiName, this);
                }
                catch (KeyNotFoundException)
                {
                    const string errmsg = "Failed to change to AI to `{0}` for NPC `{1}` - AI not found.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, aiName, this);
                    Debug.Fail(string.Format(errmsg, aiName, this));
                    return;
                }

                Debug.Assert(newAI.Actor == this);
                _ai = newAI;
            }
        }

        /// <summary>
        /// Updates the NPC
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Check for spawning if dead
            if (!IsAlive)
                return;

            // Update the AI
            AIBase ai = AI;
            if (ai != null)
                ai.Update();

            // Perform the base update of the character
            base.Update(imap, deltaTime);
        }

        #region IRespawnable Members

        public bool ReadyToRespawn(int currentTime)
        {
            return IsAlive || currentTime > _respawnTime;
        }

        void IRespawnable.Respawn()
        {
            // Restore the NPC's stats
            Stats[StatType.HP] = Stats[StatType.MaxHP];
            Stats[StatType.MP] = Stats[StatType.MaxMP];

            // Set the NPC's new location
            Teleport(new Vector2(560f, 400f)); // HACK: Hard-coded spawn location

            // Set the NPC as alive
            IsAlive = true;

            // Set the map
            if (RespawnMap == null)
            {
                // If the respawn map is invalid, there is nothing we can do to spawn it, so dispose of it
                const string errmsg = "Null respawn map for NPC `{0}`. Cannot respawn - disposing instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                DelayedDispose();
            }
            else
            {
                if (RespawnMap != null)
                    ChangeMap(RespawnMap);
            }
        }

        DynamicEntity IRespawnable.DynamicEntity
        {
            get { return this; }
        }

        #endregion
    }
}