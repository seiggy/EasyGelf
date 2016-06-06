using System.Threading.Tasks;

namespace EasyGelf.Core.Encoders
{
    public interface IChunkedMessageIdGenerator
    {
        Task<byte[]> GenerateId(byte[] message);
    }
}