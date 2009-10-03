using System.Linq;
using System.Net;
using NetGore;

namespace NetGore.Network
{
    /// <summary>
    /// A struct that joins a received packet with the EndPoint that it came from.
    /// </summary>
    public struct AddressedPacket
    {
        /// <summary>
        /// The received packet data.
        /// </summary>
        public readonly byte[] Data;

        /// <summary>
        /// The EndPoint that the Data came from.
        /// </summary>
        public readonly EndPoint RemoteEndPoint;

        /// <summary>
        /// AddressedPacket constructor.
        /// </summary>
        /// <param name="data">The received packet data.</param>
        /// <param name="remoteEndPoint">The EndPoint that the Data came from.</param>
        public AddressedPacket(byte[] data, EndPoint remoteEndPoint)
        {
            Data = data;
            RemoteEndPoint = remoteEndPoint;
        }
    }
}