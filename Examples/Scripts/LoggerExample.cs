using Godot;

namespace AetherHex.Utils.Logger.Examples
{
	public partial class LoggerExample : Node
	{
		[ExportGroup("Message Textbox")]
		[Export] private ColorPickerButton messageColorPickerBtn;
		[Export] private LineEdit messageLineEdit;
		[ExportGroup("Buttons")]
		[Export] private Button customLogBtn;
		[Export] private Button errorLogBtn;
		[Export] private Button warningLogBtn;
		[Export] private Button infoLogBtn;

		public override void _Ready()
		{
			// Initial test
			Log.Message("Logger is active and running.")
				.Bold()
				.Color(Colors.LemonChiffon)
				.Push();

			// Subscribe button events to custom log calls
			customLogBtn.Pressed += CustomLog_Pressed;
			errorLogBtn.Pressed += () => { Log.Error(messageLineEdit.Text);};
			warningLogBtn.Pressed += () => { Log.Warning(messageLineEdit.Text);};
			infoLogBtn.Pressed += () => { Log.Info(messageLineEdit.Text);};
		}

		private void CustomLog_Pressed()
		{
			Color customColor = messageColorPickerBtn.Color;
			string customContent = messageLineEdit.Text;
			Log.Message(customContent).Color(customColor).Push();
		}

		
	}
}