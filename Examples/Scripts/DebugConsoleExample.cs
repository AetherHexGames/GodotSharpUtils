using Godot;

namespace AetherHex.Utils.Debugger.Examples
{
	public partial class DebugConsoleExample : Node
	{
		[Export] private PackedScene debugConsole;

		public override void _Ready()
		{
			var instance = debugConsole.Instantiate() as CanvasLayer;
			AddChild(instance);
		}
	}
}