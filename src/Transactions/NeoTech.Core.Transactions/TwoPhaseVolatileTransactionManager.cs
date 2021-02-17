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
			if (Transaction.Current != null)
			{
				Transaction.Current.EnlistVolatile(GetEnlistmentNotification(), EnlistmentOptions.None);


			}
		}

		protected virtual IEnlistmentNotification GetEnlistmentNotification()
		{
			return new TwoPhaseEnlistmentNotification(this);
		}
	}
}