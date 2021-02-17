using System;
using System.Collections.Generic;

namespace NeoTech.Core.Transactions
{
	public interface ITransactionManager
	{
		IList<Action> OnCompletionActions { get; }

		IList<Action> OnRollbackActions { get; }

		IList<Action> OnExecuteActions { get; }

		void TrySubscribe();

		bool IsSubscribed { get; }
	}
}
