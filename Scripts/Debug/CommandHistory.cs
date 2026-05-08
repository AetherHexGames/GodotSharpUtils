using Godot;

namespace AetherHex.Utils.Debugger
{
	/// <summary>
	/// Manages a navigable history of commands entered in the debug console.
	/// Supports cycling through previous and next entries.
	/// </summary>
	public partial class CommandHistory : Node
	{
		private Godot.Collections.Array<string> _history = new();
		private int _historyIndex = -1;

		/// <summary>
		/// Adds a command to the front of the history and resets the navigation index.
		/// </summary>
		/// <param name="cmd">The command string to store.</param>
		public void PushCommand(string cmd)
		{
			_history.Insert(0, cmd);
			_historyIndex = -1;
		}

		/// <summary>
		/// Returns the previous command in history (moving backward).
		/// Clamps the index to valid range.
		/// </summary>
		/// <returns>The previous command string, or empty if history is empty.</returns>
		public string GetPreviousCommand()
		{
			if (_history.Count == 0)
				return string.Empty;
			_historyIndex = Mathf.Clamp(_historyIndex + 1, 0, _history.Count - 1);
			return _history[_historyIndex];
		}

		/// <summary>
		/// Returns the next command in history (moving forward).
		/// Clamps the index to valid range.
		/// </summary>
		/// <returns>The next command string, or empty if history is empty.</returns>
		public string GetNextCommand()
		{
			if (_history.Count == 0)
				return string.Empty;
			_historyIndex = Mathf.Clamp(_historyIndex - 1, 0, _history.Count - 1);
			return _history[_historyIndex];
		}
	}
}