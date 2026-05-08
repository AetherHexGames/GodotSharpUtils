using System.Collections.Generic;
using System.Linq;
using Godot;
using GDict = Godot.Collections.Dictionary;
using AetherHex.Utils.Logger;
using System;
using System.Text.RegularExpressions;

namespace AetherHex.Utils.Debugger
{
	/// <summary>
	/// Provides tab-based auto-completion for debug console commands by
	/// matching user input against registered method names. Repeated presses
	/// cycle through all available matches.
	/// </summary>
	public partial class AutoComplete : Node
	{
		private int autoCompleteIndex = 0;
		private string[] autoCompleteMethods;
		private bool lastInputWasAutocomplete = false;
		private string[] prevAutocompleteMatches;

		/// <summary>
		/// Initializes auto-completion with the list of available console methods.
		/// </summary>
		/// <param name="consoleMethods">
		/// A list of method dictionaries (from ConsoleCommands.GetScriptMethodList)
		/// whose "name" keys provide the auto-completion candidates.
		/// </param>
		public AutoComplete(List<GDict> consoleMethods)
		{
			autoCompleteMethods = consoleMethods.Select(x => x["name"].AsString()).ToArray();
		}

		/// <summary>
		/// Returns the best auto-complete match for the given input string.
		/// On repeated calls with the same exact match, cycles to the next
		/// candidate. Falls back to the original input if no match is found.
		/// </summary>
		/// <param name="input">The current text entered in the console line edit.</param>
		/// <returns>The auto-completed method name, or the original input if no match exists.</returns>
		public string Get(string input)
		{
			var matches = new List<string>();
			var matchString = input.Replace("()","");

			if (matchString.Length == 0)
			{
				matches = autoCompleteMethods.ToList();
			}
			else
			{
				foreach (var method in autoCompleteMethods)
				{
					if(method == matchString)
					{
						matches = prevAutocompleteMatches.ToList();
						lastInputWasAutocomplete = true;
						break;
					}
					if (method.StartsWith(matchString))
					{
						matches.Add(method);
					}
				}
			}

			prevAutocompleteMatches = matches.ToArray();

			if (matches.Count == 0)
			{
				return input;
			}

			if(lastInputWasAutocomplete)
			{
				autoCompleteIndex = (autoCompleteIndex + 1) % matches.Count;
			}
			else
			{
				lastInputWasAutocomplete = false;
				autoCompleteIndex = 0;
			}

			Log.Info($"Autocomplete matches ({matches.Count}) to -> {matches[autoCompleteIndex]}");
			return matches[autoCompleteIndex];
		}
	}
}