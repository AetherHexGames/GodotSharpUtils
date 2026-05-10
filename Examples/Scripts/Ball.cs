using Godot;
public partial class Ball : Node2D, IPoolable
{
    [Export] private float speed = 300f;
    public Node PoolNode => this;
    public bool InsidePool => !Visible;
    private Timer timer;
    
    public void Pool(bool pooled) => Visible = !pooled;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += EndOfLifetime;
        VisibilityChanged += () => { timer.Start(); };
    }

    public override void _Process(double delta)
    {
        Translate(Transform.Y * speed * (float)delta);
    }

    private void EndOfLifetime() => Pool(true);
}