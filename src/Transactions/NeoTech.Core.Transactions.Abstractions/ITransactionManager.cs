using System;

namespace NeoTech.Core.Transactions
{
	public interface ITransactionManager
	{
		void AddCompletionCallback(Action callback);

		void AddRollbackCallback(Action callback);

		void AddExecuteCallback(Action callback);

		bool TrySubscribe();
	}
}