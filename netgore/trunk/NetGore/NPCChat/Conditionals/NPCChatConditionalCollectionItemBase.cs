using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes an item used in the NPCChatConditionalCollectionBase, which contains the conditional to use
    /// and the values to use with it.
    /// </summary>
    /// <typeparam name="TUser">The Type of User.</typeparam>
    /// <typeparam name="TNPC">The Type of NPC.</typeparam>
    public abstract class NPCChatConditionalCollectionItemBase<TUser, TNPC> where TUser : class where TNPC : class
    {
        /// <summary>
        /// Reads the values for this NPCChatConditionalCollectionItemBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public void Read(IValueReader reader)
        {
            bool not = reader.ReadBool("Not");
            string conditionalName = reader.ReadString("ConditionalName");
            byte parameterCount = reader.ReadByte("ParameterCount");

            var parameterReaders = reader.ReadNodes("Parameter", parameterCount);
            NPCChatConditionalParameter[] parameters = new NPCChatConditionalParameter[parameterCount];
            foreach (var r in parameterReaders)
            {
                byte index = r.ReadByte("Index");
                var parameter = NPCChatConditionalParameter.Read(r);
                parameters[index] = parameter;
            }

            var conditional = NPCChatConditionalBase<TUser, TNPC>.GetConditional(conditionalName);

            if (conditional == null)
                throw new Exception(string.Format("Failed to get conditional `{0}`.", conditionalName));

            SetReadValues(conditional, not, parameters);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="not">The Not value.</param>
        /// <param name="parameters">The parameters.</param>
        protected abstract void SetReadValues(NPCChatConditionalBase<TUser, TNPC> conditional, bool not, NPCChatConditionalParameter[] parameters);

        /// <summary>
        /// Writes the NPCChatConditionalCollectionItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Not", Not);
            writer.Write("ConditionalName", Conditional.Name);
            writer.Write("ParameterCount", (byte)Parameters.Count());

            for (int i = 0; i < Parameters.Length; i++)
            {
                writer.WriteStartNode("Parameter");
                writer.Write("Index", (byte)i);
                Parameters[i].Write(writer);
                writer.WriteEndNode("Parameter");
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets a boolean that, if true, the result of this conditional
        /// when evaluating will be flipped. That is, True becomes False and vise versa. If false, the
        /// evaluated value is unchanged.
        /// </summary>
        public abstract bool Not { get; }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public abstract NPCChatConditionalBase<TUser, TNPC> Conditional { get; }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public abstract NPCChatConditionalParameter[] Parameters { get; }

        /// <summary>
        /// Evaluates the conditional using the supplied values.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>The result of the conditional's evaluation.</returns>
        public bool Evaluate(TUser user, TNPC npc)
        {
            bool ret = Conditional.Evaluate(user, npc, Parameters);

            if (Not)
                ret = !ret;

            return ret;
        }
    }
}
