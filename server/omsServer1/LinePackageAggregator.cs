using Matriks.Oms.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = Matriks.Oms.Core.Network.Buffer;

namespace omsServer1
{
    internal class LinePacketAggregator : IPacketAggregator
    {
        IBuffer _buffer;
        public PacketStatus Collect(out byte[] outputData)
        {
            outputData = _buffer.GetArray(1024);
            return PacketStatus.Done;
        }

        public void SetBufferManager(IBuffer buffer)
        {
            _buffer = buffer;
        }
    }
}