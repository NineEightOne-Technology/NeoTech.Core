using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	public class TwoPhaseVolatileTransactionManager : ITransactionManager, IEnlistmentNotification
	{
		protected Enlistment CurrentEnlistment { get; set; }

		protected bool InFinalPhase { get; set; }

		protected IList<Action> OnCompletionActions { get; }
			= new List<Action>();

		protected IList<Action> OnRollbackActions { get; }
			= new List<Action>();

		protected IList<Action> OnExecuteActions { get; }
			= new List<Action>();

		public void AddCompletionCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnCompletionActions.Add(callback);
		}

		public void AddRollbackCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnRollbackActions.Add(callback);
		}

		public void AddExecuteCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnExecuteActions.Add(callback);
		}

		public bool TrySubscribe()
		{
			if (InFinalPhase)
				return false;

			if (CurrentEnlistment == null)
			{
				var currentTransaction = Transaction.Current;

				if (currentTransaction != null)
					CurrentEnlistment = Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
			}

			return CurrentEnlistment != null;
		}

		protected virtual void OnCommitInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var completionAction in OnCompletionActions)
				completionAction();

			enlistment.Done();
		}

		protected virtual void OnInDoubtInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			enlistment.Done();
		}

		protected virtual void OnPrepareInternal(PreparingEnlistment preparingEnlistment)
		{
			Requires.NotNull(preparingEnlistment, nameof(preparingEnlistment));

			InFinalPhase = true;

			try
			{
				foreach (var executeAction in OnExecuteActions)
					executeAction();

				preparingEnlistment.Prepared();
			}
			catch (Exception e)
			{
				preparingEnlistment.ForceRollback(e);
			}
		}

		protected virtual void OnRollbackInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var rollbackAction in OnRollbackActions)
				rollbackAction();

			enlistment.Done();
		}

		void IEnlistmentNotification.Commit(Enlistment enlistment)
		{
			OnCommitInternal(enlistment);
		}

		void IEnlistmentNotification.InDoubt(Enlistment enlistment)
		{
			OnInDoubtInternal(enlistment);
		}

		void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
		{
			OnPrepareInternal(preparingEnlistment);
		}

		void IEnlistmentNotification.Rollback(Enlistment enlistment)
		{
			OnRollbackInternal(enlistment);
		}
	}
}