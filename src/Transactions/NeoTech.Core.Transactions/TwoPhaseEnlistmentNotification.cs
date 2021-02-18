using System;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	internal class TwoPhaseEnlistmentNotification : IEnlistmentNotification
	{
		public TwoPhaseEnlistmentNotification(ITransactionManager parent)
		{
			Requires.NotNull(parent, nameof(parent));

			Parent = parent;
		}

		protected ITransactionManager Parent { get; }

		public virtual void Commit(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var completionAction in Parent.OnCompletionActions)
				completionAction();

			enlistment.Done();
		}

		public virtual void InDoubt(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			enlistment.Done();
		}

		public virtual void Prepare(PreparingEnlistment preparingEnlistment)
		{
			Requires.NotNull(preparingEnlistment, nameof(preparingEnlistment));

			try
			{
				foreach (var executeAction in Parent.OnExecuteActions)
					executeAction();

				preparingEnlistment.Prepared();
			}
			catch (Exception e)
			{
				preparingEnlistment.ForceRollback(e);
			}
		}

		public virtual void Rollback(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var rollbackAction in Parent.OnRollbackActions)
				rollbackAction();

			enlistment.Done();
		}
	}
}