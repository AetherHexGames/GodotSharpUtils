using Godot;

namespace AetherHex.Utils.Logger
{
	/// <summary>
	///  Autoload class that prints custom messages, errors and warnings with a single argument
	/// </summary>
	public partial class Log : Node
	{
		private static bool isEnabled = true;
		public override void _Ready()
		{
			// Disable logger in Release builds
			if (!OS.IsDebugBuild())
			{
				isEnabled = false;
			}
		}

		/// <summary>
		/// Returns a LogMessage object that contains text in log format.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static LogMessage Message(string text)
		{
			if (!isEnabled) return null;
			text = $"{GetFormattedTime()} : {text}";
			LogMessage message = new LogMessage(text);
			return message;
		}

		/// <summary>
		/// Pushes info log to the GD Console.
		/// </summary>
		/// <param name="infoText"></param>
		public static void Info(string infoText)
		{
			if (!isEnabled) return;
			Message(infoText).Info().Push();
		}

		/// <summary>
		/// Pushes warning log to the GD Console.
		/// </summary>
		/// <param name="warnText"></param>
		public static void Warning(string warnText)
		{
			if (!isEnabled) return;
			Message(warnText).Warning().Push();
		}

		/// <summary>
		/// Pushes error log to the GD Console. 
		/// </summary>
		/// <param name="errText"></param>
		public static void Error(string errText)
		{
			if (!isEnabled) return;
			GD.PrintErr(Message(errText).Error().Output);
		}


		/// <summary>
		/// Returns a formatted string of the current time in hour, minutes and seconds
		/// </summary>
		/// <returns></returns>
		private static string GetFormattedTime()
		{
			float unix_time = (float)Time.GetUnixTimeFromSystem();
			var time_zone = Time.GetTimeZoneFromSystem();
			unix_time += (float)time_zone["bias"] * 60;
			var datetime = Time.GetDatetimeDictFromUnixTime((int)unix_time);
			datetime["millisecond"] = (int)(unix_time * 1000) % 1000;
			string result = "hh:mm:ss";
			result = result.Replace("YYYY", string.Format("{0:D4}", datetime["year"]));
			result = result.Replace("MM", string.Format("{0:D2}", datetime["month"]));
			result = result.Replace("DD", string.Format("{0:D2}", datetime["day"]));
			result = result.Replace("hh", string.Format("{0:D2}", datetime["hour"]));
			result = result.Replace("mm", string.Format("{0:D2}", datetime["minute"]));
			result = result.Replace("ss", string.Format("{0:D2}", datetime["second"]));
			result = result.Replace("SSS", string.Format("{0:D3}", datetime["millisecond"]));
			return result;
		}
	}
}

