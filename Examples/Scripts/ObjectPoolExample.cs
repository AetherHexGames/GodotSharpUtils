using Godot;

namespace AetherHex.Utils.Common.Examples
{
    public partial class ObjectPoolExample : Node
    {
        [Export] Node2D movingSpawnPoint;
        private ObjectPool objectPool;
        public override void _Ready()
        {
            objectPool = GetNode<ObjectPool>("ObjectPool_Balls");
            SpawnBall(true);
        }

        private void SpawnBall(bool setLoop = false)
        {
            var ball = objectPool.Spawn();
            var node = (Node2D)ball.PoolNode;
            node.Position = movingSpawnPoint.GlobalPosition;

            if (!setLoop) return;
            GetTree().CreateTimer(0.1f).Timeout += () =>
            {
                SpawnBall(true);
            };
        }
    }
}