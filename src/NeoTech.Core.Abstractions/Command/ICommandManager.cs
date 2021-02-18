using System;
using System.Collections.Generic;

namespace NeoTech.Core.Command
{
	/// <summary>
	/// Indicates that the implementing entity acts as a manager of commands that can be executed.
	/// </summary>
	public interface ICommandManager : IReadOnlyCollection<Action>
	{
		/// <summary>
		/// Adds an <see cref="Action"/> to this command manager.
		/// </summary>
		/// <param name="commandAction"></param>
		void AddCommand(Action commandAction);

		/// <summary>
		/// Executes all added <see cref="Action"/>s.
		/// </summary>
		void Execute();

		/// <summary>
		/// Clears all added commands from this command manager.
		/// </summary>
		void Reset();
	}
}