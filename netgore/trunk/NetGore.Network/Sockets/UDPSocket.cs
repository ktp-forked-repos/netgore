﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;

namespace NetGore.Network
{
    /// <summary>
    /// A single, basic, thread-safe socket that uses UDP. Each UDPSocket will both send and listen on the
    /// same port it is created on.
    /// </summary>
    public class UDPSocket : IDisposable
    {
        /// <summary>
        /// Length of the custom packet header in bytes.
        /// </summary>
        const int _headerSize = 0;

        /// <summary>
        /// Length of the maximum packet size in bytes.
        /// </summary>
        const int _maxPacketSize = 1024 - _headerSize;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Buffer for receiving data.
        /// </summary>
        readonly byte[] _receiveBuffer;

        /// <summary>
        /// Queue of received data that has yet to be handled.
        /// </summary>
        readonly Queue<byte[]> _receiveQueue = new Queue<byte[]>(2);

        /// <summary>
        /// Underlying Socket used by this UDPSocket.
        /// </summary>
        readonly Socket _socket;

        /// <summary>
        /// EndPoint the Socket binded to.
        /// </summary>
        EndPoint _bindEndPoint;

        /// <summary>
        /// The port used by this UDPSocket.
        /// </summary>
        int _port;

        /// <summary>
        /// EndPoint for the last packet received.
        /// </summary>
        EndPoint _remoteEndPoint = new IPEndPoint(0, 0);

        /// <summary>
        /// UDPSocket constructor.
        /// </summary>
        public UDPSocket()
        {
            _receiveBuffer = new byte[_maxPacketSize + _headerSize];
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// Prefixes the header to a packet.
        /// </summary>
        /// <param name="data">Packet to add the header to.</param>
        /// <param name="length">Length of the packet in bytes.</param>
        /// <returns>Byte array containing the <paramref name="data"/> with the packet header prefixed.</returns>
        static byte[] AddHeader(byte[] data, ushort length)
        {
            // No headers needed right now
            return data;
        }

        /// <summary>
        /// Starts the receiving.
        /// </summary>
        void BeginReceiveFrom()
        {
            _socket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref _bindEndPoint,
                                     ReceiveFromCallback, this);
        }

        /// <summary>
        /// Changes the Port for this UDPSocket.
        /// </summary>
        /// <returns>Port that the UDPSocket bound to.</returns>
        public int Bind()
        {
            // NOTE: This will probably crash if the port is already set

            // Close down the old connection
            if (_socket.IsBound)
                _socket.Disconnect(true);

            // Bind
            _bindEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _socket.Bind(_bindEndPoint);

            // Get the port
            EndPoint endPoint = _socket.LocalEndPoint;
            if (endPoint == null)
            {
                const string errmsg = "Failed to bind the UDPSocket!";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                Debug.Fail(errmsg);
                throw new Exception(errmsg);
            }
            _port = ((IPEndPoint)endPoint).Port;

            // Begin receiving
            BeginReceiveFrom();

            return _port;
        }

        /// <summary>
        /// Gets the queued data received by this UDPSocket.
        /// </summary>
        /// <returns>The queued data received by this UDPSocket, or null if empty.</returns>
        public byte[][] GetRecvData()
        {
            lock (_receiveQueue)
            {
                int length = _receiveQueue.Count;
                if (length == 0)
                    return null;

                var packets = _receiveQueue.ToArray();
                _receiveQueue.Clear();
                return packets;
            }
        }

        /// <summary>
        /// Callback for ReceiveFrom.
        /// </summary>
        /// <param name="result">Async result.</param>
        void ReceiveFromCallback(IAsyncResult result)
        {
            byte[] received = null;

            try
            {
                // Read the received data and put it into a temporary buffer
                int bytesRead = _socket.EndReceiveFrom(result, ref _remoteEndPoint);
                received = new byte[bytesRead];
                Buffer.BlockCopy(_receiveBuffer, 0, received, 0, bytesRead);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Received {0} bytes from {1}", bytesRead, _remoteEndPoint);
            }
            catch (SocketException e)
            {
                if (log.IsErrorEnabled)
                    log.Error(e);
                Debug.Fail(e.ToString());
            }
            finally
            {
                // Start receiving again
                BeginReceiveFrom();
            }

            // Push the received data into the receive queue
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (received != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                lock (_receiveQueue)
                    _receiveQueue.Enqueue(received);
            }
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="length">Length of the data to send in bytes.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, int length, EndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (length > _maxPacketSize)
                throw new ArgumentOutOfRangeException("data", "Data is too large to send.");

            data = AddHeader(data, (ushort)length);
            _socket.SendTo(data, data.Length + _headerSize, SocketFlags.None, endPoint);

            if (log.IsInfoEnabled)
                log.InfoFormat("Sent `{0}` bytes to `{1}`", length, endPoint);
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, EndPoint endPoint)
        {
            Send(data, data.Length, endPoint);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_socket != null)
                _socket.Close();
        }

        #endregion
    }
}