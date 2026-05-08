using System.Collections;
using System.Threading.Tasks;
using Godot;

public static class CanvasUtils
{
    public static void ModulateAlpha(Control node, float targetAlpha, float time = 1f)
    {
        var tween = node.GetTree().CreateTween();
        tween.TweenProperty(node, "modulate:a", targetAlpha, time).Finished += () =>
        {
            tween.Kill();
        };
    }
}
