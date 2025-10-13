using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using OpenAutoBench_ng.OpenAutoBench;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace OpenAutoBench_ng.Communication.Instrument.Connection
{
    public class VISAConnection : IInstrumentConnection
    {
        private string _resourceName { get; set; }
        private IMessageBasedSession _mb { get; set; }
        public VISAConnection(string resourceName)
        {
            _resourceName = resourceName;
        }

        public void Connect()
        {
            using (ResourceManager rm = new ResourceManager())
            {
                _mb = (MessageBasedSession)rm.Open(_resourceName);
            }
        }

        public void Disconnect()
        {
            _mb.Dispose();
        }
        
        /// <summary>
        /// Send a VISA command and expect a response
        /// </summary>
        /// <param name="toSend"></param>
        /// <returns></returns>
        public async Task<string> Send(string toSend)
        {
            // Send
            _mb.RawIO.Write(toSend);
            // Get response
            return _mb.RawIO.ReadString();
        }

        public async Task Transmit(string toSend)
        {
            _mb.RawIO.Write(toSend);
        }

        public async Task TransmitByte(byte[] toSend)
        {
            _mb.RawIO.Write(toSend, 0, toSend.Length);
        }

        public async Task<string> ReadLine()
        {
            return _mb.RawIO.ReadString();
        }

        public async Task<byte[]> ReceiveByte()
        {
            throw new NotImplementedException();
        }

        public async Task FlushBuffer()
        {
            _mb.Clear();
        }

        public void SetDelimeter(string delimeter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a list of all available GPIB resources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetGPIBResources()
        {
            using (ResourceManager rm = new ResourceManager())
                return rm.Find("?*INSTR");
        }
    }
}
