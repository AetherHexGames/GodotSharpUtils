using Godot;
using System.Reflection;
using Godot.Collections;
using System.Collections.Generic;
using System;

namespace AetherHex.Utils.Debugger
{
	/// <summary>
	/// Provides command processing and method discovery for the debug console.
	/// Custom commands can be added via the CustomFunctions region.
	/// </summary>
	public partial class ConsoleCommands : Node
	{
		private RichTextLabel output;

		/// <summary>
		/// Creates a new ConsoleCommands instance bound to the given output label.
		/// </summary>
		/// <param name="output">The RichTextLabel where console output will be displayed.</param>
		public ConsoleCommands(RichTextLabel output)
		{
			this.output = output;
		}

		/// <summary>
		/// Clears all text from the console output.
		/// </summary>
		public void ClearConsole() => output.Text = String.Empty;

		/// <summary>
		/// Appends a list of all available command names to the console output.
		/// </summary>
		public void DisplayCommands()
		{
			output.Text += "\nCommands: \n";

			foreach (var method in GetScriptMethodList())
			{
				output.Text += $"{method["name"]}() \n";
			}
		}

		#region CustomFunctions
		// Drop your custom functions here
		#endregion

		/// <summary>
		/// Retrieves a list of all public instance methods declared on this class,
		/// excluding Godot built-in methods, formatted as dictionaries suitable for
		/// Godot's method reflection system.
		/// </summary>
		/// <returns>A list of dictionaries each containing method name, return type, flags, and parameters.</returns>
		public List<Dictionary> GetScriptMethodList()
		{
			var result = new List<Dictionary>();
			foreach (var method in GetType().GetMethods(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				// Skip Godot built-in methods
        		if (method.DeclaringType?.Namespace?.StartsWith("Godot") == true)
            	continue;

				var info = new Dictionary
				{
					{ "name", method.Name },
					{ "return_type", method.ReturnType.Name },
					{ "flags", (int)((method.IsStatic ? MethodFlags.Static : 0) |
								(method.IsVirtual ? MethodFlags.Virtual : 0)) }
				};

				var args = new Godot.Collections.Array();
				foreach (var param in method.GetParameters())
				{
					args.Add(new Godot.Collections.Dictionary {
					{ "name", param.Name },
					{ "type", param.ParameterType.Name }
				});
				}

				info["args"] = args;
				result.Add(info);
			}
			return result;
		}
	}
}