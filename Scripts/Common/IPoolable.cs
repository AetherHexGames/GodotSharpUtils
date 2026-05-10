using Godot;

namespace AetherHex.Utils.Common
{
    public interface IPoolable
    {
        Node PoolNode { get; }
        bool InsidePool { get; }
        void Pool(bool poolNode);
    }
}