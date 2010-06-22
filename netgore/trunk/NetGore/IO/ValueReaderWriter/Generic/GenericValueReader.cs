﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// An implementation of an <see cref="IValueReader"/> that allows you to specify during the construction of the object
    /// what format to use instead of specifying a specific <see cref="IValueReader"/> and <see cref="IValueWriter"/> directly.
    /// The results are the exact same as if the underlying <see cref="IValueReader"/> and <see cref="IValueWriter"/> are
    /// used directly. Because of this, you can, for example, use a <see cref="GenericValueReader"/> to read a file written with an
    /// <see cref="XmlValueWriter"/> as long as the <see cref="GenericValueReader"/> can recognize the format. However, it is
    /// recommended that you always use a <see cref="GenericValueReader"/> and <see cref="GenericValueWriter"/> directly when
    /// possible to avoid any issues with format recognition and provide easier alteration of the used formats in the future.
    /// </summary>
    public class GenericValueReader : IValueReader
    {
        /// <summary>
        /// The maximum length in chars of any of the headers. All formats must be identified using no more than this many chars.
        /// </summary>
        const int _maxHeaderLength = 8;

        /// <summary>
        /// Contains the bytes used to recognize an Xml header.
        /// </summary>
        static readonly char[] _xmlHeader = new char[] { '<','?','x','m','l',' ' };

        readonly IValueReader _reader;

        /// <summary>
        /// Initializes the <see cref="GenericValueReader"/> class.
        /// </summary>
        static GenericValueReader()
        {
            // Make sure header identifiers are a valid length
            Debug.Assert(_xmlHeader.Length <= _maxHeaderLength);
        }

        /// <summary>
        /// Checks if the header bytes are equal to the expected bytes.
        /// </summary>
        /// <param name="header">The read header bytes.</param>
        /// <param name="headerLength">The actual length of the header.</param>
        /// <param name="expected">The expected bytes for the header.</param>
        /// <returns>True if the <paramref name="header"/> matches the <paramref name="expected"/>; otherwise false.</returns>
        static bool CheckFormatHeader(char[] header, int headerLength, char[] expected)
        {
            if (headerLength < expected.Length)
                return false;

            for (int i = 0; i < expected.Length; i++)
            {
                if (header[i] != expected[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Peeks into a file to figure out what format it uses.
        /// </summary>
        /// <param name="filePath">The path to the file to check.</param>
        /// <returns>The <see cref="GenericValueIOFormat"/> being used for the <paramref name="filePath"/>.</returns>
        public static GenericValueIOFormat FindFileFormat(string filePath)
        {
            // Grab enough characters from the file to determine the format
            char[] header = new char[_maxHeaderLength];
            int headerLength;
            using (var fs = new StreamReader(filePath, true))
            {
                headerLength = fs.Read(header, 0, header.Length);
            }

            // Check for Xml
            if (CheckFormatHeader(header, headerLength, _xmlHeader))
                return GenericValueIOFormat.Xml;

            // Assume everything else is binary
            return GenericValueIOFormat.Binary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericValueReader"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file to load.</param>
        /// <param name="rootNodeName">The name of the root node. Not used by all formats, but should always be included anyways.</param>
        /// <param name="useEnumNames">Whether or not enum names should be used. If true, enum names will always be used. If false, the
        /// enum values will be used instead. If null, the default value for the underlying <see cref="IValueReader"/> will be used.</param>
        /// <exception cref="FileLoadException"><paramref name="filePath"/> contains an unsupported format.</exception>
        public GenericValueReader(string filePath, string rootNodeName, bool? useEnumNames = null)
        {
            // Discover the format
            var format = FindFileFormat(filePath);
            Debug.Assert(EnumHelper<GenericValueIOFormat>.IsDefined(format));

            // Create the IValueReader of the needed type
            switch (format)
            {
                case GenericValueIOFormat.Binary:
                    if (useEnumNames.HasValue)
                        _reader = new BinaryValueReader(filePath, useEnumNames.Value);
                    else
                        _reader = new BinaryValueReader(filePath);
                    break;

                case GenericValueIOFormat.Xml:
                    if (useEnumNames.HasValue)
                        _reader = new XmlValueReader(filePath, rootNodeName, useEnumNames.Value);
                    else
                        _reader = new XmlValueReader(filePath, rootNodeName);
                    break;

                default:
                    const string errmsg = "Ran into unsupported format `{0}`. Format value was acquired from FindFileFormat().";
                    Debug.Fail(string.Format(errmsg, format));
                    throw new FileLoadException(string.Format(errmsg, format));
            }

            Debug.Assert(_reader != null);
        }

        /// <summary>
        /// Gets if this <see cref="IValueReader"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return _reader.SupportsNameLookup; }
        }

        /// <summary>
        /// Gets if this <see cref="IValueReader"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        public bool SupportsNodes
        {
            get { return _reader.SupportsNodes; }
        }

        /// <summary>
        /// Gets if Enum I/O will be done with the Enum's name. If true, the name of the Enum value instead of the
        /// underlying integer value will be used. If false, the underlying integer value will be used. This
        /// only to Enum I/O that does not explicitly state which method to use.
        /// </summary>
        public bool UseEnumNames
        {
            get { return _reader.UseEnumNames; }
        }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public bool ReadBool(string name)
        {
            return _reader.ReadBool(name);
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public byte ReadByte(string name)
        {
            return _reader.ReadByte(name);
        }

        /// <summary>
        /// Reads a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public double ReadDouble(string name)
        {
            return _reader.ReadDouble(name);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or the
        /// name of the Enum value is determined from the <see cref="IValueReader.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return _reader.ReadEnum<T>(name);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return _reader.ReadEnumName<T>(name);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's value instead of the name.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return _reader.ReadEnumValue<T>(name);
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public float ReadFloat(string name)
        {
            return _reader.ReadFloat(name);
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name)
        {
            return _reader.ReadInt(name);
        }

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name, int bits)
        {
            return _reader.ReadInt(name, bits);
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public long ReadLong(string name)
        {
            return _reader.ReadLong(name);
        }

        /// <summary>
        /// Reads multiple values that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of value to read.</typeparam>
        /// <param name="nodeName">The name of the node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        public T[] ReadMany<T>(string nodeName, ReadManyHandler<T> readHandler)
        {
            return _reader.ReadMany(nodeName, readHandler);
        }

        /// <summary>
        /// Reads multiple nodes that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">The name of the root node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        public T[] ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler)
        {
            return _reader.ReadManyNodes(nodeName, readHandler);
        }

        /// <summary>
        /// Reads multiple nodes that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">The name of the root node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <param name="handleMissingKey">Allows for handling when a key is missing or invalid instead of throwing
        /// an <see cref="Exception"/>. This allows nodes to be read even if one or more of the expected
        /// items are missing. The returned array will contain null for these indicies. The int contained in the
        /// <see cref="Action{T}"/> contains the 0-based index of the index that failed. This parameter is only
        /// valid when <see cref="IValueReader.SupportsNameLookup"/> and <see cref="IValueReader.SupportsNodes"/> are true.
        /// Default is null.</param>
        /// <returns>
        /// Array of the values read from the IValueReader.
        /// </returns>
        public T[] ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler, Action<int, Exception> handleMissingKey)
        {
            return _reader.ReadManyNodes(nodeName, readHandler, handleMissingKey);
        }

        /// <summary>
        /// Reads a single child node, while enforcing the idea that there should only be one node
        /// in the key. If there is more than one node for the given <paramref name="key"/>, an
        /// ArgumentException will be thrown.
        /// </summary>
        /// <param name="key">The key of the child node to read.</param>
        /// <returns>An IValueReader to read the child node.</returns>
        /// <exception cref="ArgumentException">Zero or more than one values found for the given
        /// <paramref name="key"/>.</exception>
        public IValueReader ReadNode(string key)
        {
            return _reader.ReadNode(key);
        }

        /// <summary>
        /// Reads one or more child nodes from the IValueReader.
        /// </summary>
        /// <param name="name">Name of the nodes to read.</param>
        /// <param name="count">The number of nodes to read. If this value is 0, an empty IEnumerable of IValueReaders
        /// will be returned, even if the key could not be found.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count is less than 0.</exception>
        public IEnumerable<IValueReader> ReadNodes(string name, int count)
        {
            return _reader.ReadNodes(name, count);
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public sbyte ReadSByte(string name)
        {
            return _reader.ReadSByte(name);
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public short ReadShort(string name)
        {
            return _reader.ReadShort(name);
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>String read from the reader.</returns>
        public string ReadString(string name)
        {
            return _reader.ReadString(name);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name)
        {
            return _reader.ReadUInt(name);
        }

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name, int bits)
        {
            return _reader.ReadUInt(name, bits);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public ulong ReadULong(string name)
        {
            return _reader.ReadULong(name);
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public ushort ReadUShort(string name)
        {
            return _reader.ReadUShort(name);
        }
    }
}
