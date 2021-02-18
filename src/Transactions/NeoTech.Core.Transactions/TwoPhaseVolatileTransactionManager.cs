using System;
using System.Collections.Generic;
using System.Transactions;

namespace NeoTech.Core.Transactions
{
	public class TwoPhaseVolatileTransactionManager : ITransactionManager
	{
		public IList<Action> OnCompletionActions { get; }
			= new List<Action>();

		public IList<Action> OnRollbackActions { get; }
			= new List<Action>();

		public IList<Action> OnExecuteActions { get; }
			= new List<Action>();

		public bool IsSubscribed { get; internal set; }

		public void TrySubscribe()
		{
			var currentTransaction = Transaction.Current;

			if (currentTransaction != null)
			{
				currentTransaction.TransactionCompleted += OnTransactionCompleted;

				currentTransaction.EnlistVolatile(GetEnlistmentNotification(), EnlistmentOptions.None);

				IsSubscribed = true;
			}
		}

		private void OnTransactionCompleted(object sender, TransactionEventArgs _)
		{
			var transaction = sender as Transaction;

			if (transaction != null)
			{
				transaction.TransactionCompleted -= OnTransactionCompleted;

				IsSubscribed = false;
			}
		}

		protected virtual IEnlistmentNotification GetEnlistmentNotification()
		{
			return new TwoPhaseEnlistmentNotification(this);
		}
	}
}