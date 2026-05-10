using Godot;
using System.Collections.Generic;

namespace AetherHex.Utils.Common
{
    public partial class ObjectPool : Node
    {
        [Export] private PackedScene sceneToSpawn;
        [Export] private int initialAmount;

        private List<IPoolable> pool = [];

        public override void _Ready()
        {
            for (int i = 0; i < initialAmount; i++)
            {
                CreateNewNode();
            }
        }

        private IPoolable CreateNewNode()
        {
            var node = sceneToSpawn.Instantiate<IPoolable>();
            GetTree().Root.CallDeferred(Node.MethodName.AddChild, node.PoolNode);
            node.Pool(true);
            pool.Add(node);

            return node;
        }

        public IPoolable Spawn()
        {
            IPoolable node = null;

            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].InsidePool)
                {
                    node = pool[i];
                    break;
                }
            }

            node ??= CreateNewNode(); //Generate new node if pool is full
            node.Pool(false);
            return node;
        }
    }
}


