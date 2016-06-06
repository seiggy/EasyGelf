using System.IO;
using System.Net;

namespace EasyGelf.Core.Transports.Http
{
    public sealed class HttpTransport : ITransport
    {
        private readonly HttpTransportConfiguration configuration;
        private readonly IGelfMessageSerializer messageSerializer;

        public HttpTransport(HttpTransportConfiguration configuration, IGelfMessageSerializer messageSerializer)
        {
            this.configuration = configuration;
            this.messageSerializer = messageSerializer;
        }

        public void Send(GelfMessage message)
        {
            var request = (HttpWebRequest)WebRequest.Create(configuration.Uri);
            using (var requestStream = request.GetRequestStreamAsync().Result)
            using (var messageStream = new MemoryStream(messageSerializer.Serialize(message)))
                messageStream.CopyTo(requestStream);
            request.Method = "POST";
            //request.AllowAutoRedirect = false;
            request.ContinueTimeout = configuration.Timeout;
            using (var response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                if (response.StatusCode == HttpStatusCode.Accepted)
                    return;
                throw new SendFailedException();
            }
        }

        public void Close()
        {
        }
    }
}