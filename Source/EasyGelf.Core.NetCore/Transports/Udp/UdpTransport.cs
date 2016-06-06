using System.Net.Sockets;
using EasyGelf.Core.Encoders;

namespace EasyGelf.Core.Transports.Udp
{
    public sealed class UdpTransport : ITransport
    {
        private readonly UdpTransportConfiguration configuration;
        private readonly ITransportEncoder encoder;
        private readonly IGelfMessageSerializer messageSerializer;

        public UdpTransport(UdpTransportConfiguration configuration, ITransportEncoder encoder, IGelfMessageSerializer messageSerializer)
        {
            this.configuration = configuration;
            this.encoder = encoder;
            this.messageSerializer = messageSerializer;
        }

        public void Send(GelfMessage message)
        {
            using (var udpClient = new UdpClient())
                foreach (var bytes in encoder.Encode(messageSerializer.Serialize(message)))
                {
                    var result = udpClient.SendAsync(bytes, bytes.Length, configuration.Host).Result;
                }
        }

        public void Close()
        {
        }
    }
}