using System.IO;

namespace Core.GGPO
{
    public interface INetworkObject
    {
        void Serialize(BinaryWriter bw);

        void Deserialize(BinaryReader br);
    }
}