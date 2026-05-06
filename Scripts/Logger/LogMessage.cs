using Godot;
using static Godot.GD;

namespace AetherHex.Utils.Logger
{
    /// <summary>
	///  Class with constructor that has helpers to stylize log text before printing to Godot console.
    ///  Supports BBCode and uses GD.PrintRich() to push logs.
	/// </summary>
    public class LogMessage
    {
        private string content;
        public string Output => content;

        public LogMessage(string content)
        {
            this.content = content;
        }

        /// <summary>
	    ///  Pushes the stored content inside this object.
	    /// </summary>
        public void Push() => PrintRich(content);

        /// <summary>
	    ///  Formats content with error type format and pushes the error to the stacktrace in GD.
	    /// </summary>
        public LogMessage Error()
        {
            PushError(content);
            var icon = GetIconBBCode("Error", 16);
            content = $"{icon} [wave][color=red]{content}[/color][/wave]";
            return this;
        }

        /// <summary>
	    ///  Formats content with warning type format and pushes the warning to the stacktrace in GD.
	    /// </summary>
        public LogMessage Warning()
        {
            PushWarning(content);
            var icon = GetIconBBCode("Warning", 16);
            content = $"{icon} [color=yellow]{content}[/color]";
            return this;
        }

        /// <summary>
	    ///  Formats content with info type format. Useful when making annotations.
	    /// </summary>
        public LogMessage Info()
        {
            var icon = GetIconBBCode("Info", 16);
            content = $"{icon} {content}";
            return this;
        }

        /// <summary>
	    ///  Formats content into bold text.
	    /// </summary>
        /// <returns></returns>
        public LogMessage Bold()
        {
            content = $"[b]{content}[/b]";
            return this;
        }

        /// <summary>
	    ///  Formats content in the specified text color.
	    /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public LogMessage Color(Color color)
        {
            content = $"[color=#{color.ToHtml()}]{content}[/color]";
            return this;
        }

        /// <summary>
        /// Returns image in BBCode using Assets/Icons/ as path and custom size.
        /// </summary>
        /// <param name="iconName"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetIconBBCode(string iconName, int size = 16)
        {
            return $"[img={size}]res://Assets/Icons/{iconName}.svg[/img]";

        }
    }
}