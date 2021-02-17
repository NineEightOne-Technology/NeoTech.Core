using System;
using System.Collections.Generic;

namespace NeoTech.Core.Abstractions.Command
{
	public interface ICommandManager : IReadOnlyCollection<Action>
	{
		void AddCommand(Action commandAction);

		void Execute();

		void Reset();
	}
}