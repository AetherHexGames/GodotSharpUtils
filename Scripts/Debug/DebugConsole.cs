using Godot;
using AetherHex.Utils.Common;

namespace AetherHex.Utils.Debugger
{
	/// <summary>
	/// Main debug console overlay that provides command input, execution,
	/// history navigation, auto-completion, and time-scale controls.
	/// Only active in debug builds.
	/// </summary>
	public partial class DebugConsole : CanvasLayer
	{
		[Export]
		private Button toggleDebugButton;
		[Export]
		private Control consoleContainer;
		[Export]
		private LineEdit lineEdit;
		[Export]
		private RichTextLabel output;
		[Export]
		private Control extraControls;
		[Export]
		private HSlider timeScaleSlider;
		[Export]
		private Label timeScaleValue;
		[Export]
		private float defaultTimeScale = 1.0f;
		private ConsoleCommands consoleCommands;
		private CommandHistory commandHistory;
		private AutoComplete autoComplete;
		private Expression expression = new Expression();
		private bool toggleConsole = false;
		private bool consoleEnabled = false;

		/// <summary>
		/// Initializes the debug console: sets up command runner, history,
		/// auto-completion, time-scale slider, and connects input signals.
		/// In non-debug builds the console and toggle button are hidden.
		/// </summary>
		public override void _Ready()
		{
			consoleContainer.Visible = false;

			if (!OS.IsDebugBuild())
			{
				extraControls.Visible = false;
				toggleDebugButton.Visible = false;
				return;
			}

			consoleCommands = new ConsoleCommands(output);
			AddChild(consoleCommands);
			commandHistory = new CommandHistory();
			AddChild(commandHistory);
			autoComplete = new AutoComplete(consoleCommands.GetScriptMethodList());
			AddChild(autoComplete);

			extraControls.Visible = false;

			_On_Time_Slider_Value_Changed(defaultTimeScale);
			timeScaleSlider.ValueChanged += _On_Time_Slider_Value_Changed;
			toggleDebugButton.Pressed += ToggleConsole;
			consoleEnabled = true;
			CanvasUtils.ModulateAlpha(toggleDebugButton, 0f, 0.1f);
		}

		/// <summary>
		/// Toggles the visibility of the console overlay and extra controls.
		/// Fades the toggle button alpha accordingly.
		/// </summary>
		private void ToggleConsole()
		{
			if (!consoleEnabled) return;
			toggleConsole = !toggleConsole;
			consoleContainer.Visible = toggleConsole;
			extraControls.Visible = toggleConsole;
			CanvasUtils.ModulateAlpha(toggleDebugButton, toggleConsole ? 1f:0f, 0.2f);
		}

		/// <summary>
		/// Handles input events for the debug console: toggle, command submission,
		/// history navigation (previous/next), and auto-completion.
		/// </summary>
		/// <param name="event">The incoming input event.</param>
		public override void _Input(InputEvent @event)
		{
			if (!OS.IsDebugBuild()) return;
			if (Input.IsActionJustPressed("_Dev_Console_Toggle"))
				ToggleConsole();
			if (!toggleConsole)
				return;

			string cmd = lineEdit.Text;

			if (Input.IsActionJustPressed("_Dev_Console_Enter"))
			{
				commandHistory.PushCommand(cmd);
				OnRunCommand(cmd);
				lineEdit.Clear();
			}
			else if (Input.IsActionJustPressed("_Dev_Console_Prev"))
			{
				lineEdit.Text = commandHistory.GetPreviousCommand();
				lineEdit.CaretColumn = 100000;
			}
			else if (Input.IsActionJustPressed("_Dev_Console_Next"))
			{
				lineEdit.Text = commandHistory.GetNextCommand();
				lineEdit.CaretColumn = 100000;
			}
			else if (Input.IsActionJustPressed("_Dev_Console_Autocomplete"))
			{
				lineEdit.Text = $"{autoComplete.Get(cmd)}()";
				lineEdit.CaretColumn = 100000;
			}
		}

		/// <summary>
		/// Parses and executes the given command string using Godot's Expression
		/// system against the ConsoleCommands instance. Outputs the result or
		/// an error message to the console.
		/// </summary>
		/// <param name="cmd">The command string to execute.</param>
		private void OnRunCommand(string cmd)
		{
			expression = new Expression();
			var parseError = expression.Parse(cmd);
			if (parseError != Error.Ok)
			{
				output.Text += $"{parseError} \n";
				return;
			}

			var result = expression.Execute([], consoleCommands);
			if (expression.HasExecuteFailed())
			{
				output.Text += "[color=red]Command failed! Please check your command again.[/color]";
				return;
			}
			output.Text += $"{result} \n";
		}

		/// <summary>
		/// Updates the engine's time scale when the slider value changes.
		/// Displays the new time scale value in the console output.
		/// </summary>
		/// <param name="value">The new time scale value from the slider.</param>
		private void _On_Time_Slider_Value_Changed(double value)
		{
			Engine.TimeScale = value;
			timeScaleValue.Text = $"Time scale {value}";
			output.Text += $"Time scale updated to {value} \n";
		}
	}
}