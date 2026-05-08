using Godot;

namespace AetherHex.Utils.Debugger.Tests
{
	public partial class DebugConsoleTest : Node
	{
		PackedScene debugConsole = GD.Load<PackedScene>("res://Scenes/Props/DebugConsole.tscn");

		public override void _Ready()
		{
			var instance = debugConsole.Instantiate() as CanvasLayer;
			AddChild(instance);
		}
	}
}