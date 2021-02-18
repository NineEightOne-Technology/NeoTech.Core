using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	/// <summary>
	/// Acts as a transaction manager that handles two-phase-commit transactions.
	/// </summary>
	public class TwoPhaseVolatileTransactionManager : ITransactionManager, IEnlistmentNotification
	{
		/// <summary>
		/// Gets or sets the current enlistment, If this transaction manager is enlisted with a running transaction.
		/// </summary>
		protected Enlistment CurrentEnlistment { get; set; }

		/// <summary>
		/// Gets or sets an indicator if this transaction manager has entered the Commit phase.
		/// </summary>
		protected bool InFinalPhase { get; set; }

		/// <summary>
		/// Gets the list of actions to be invoked when the transaction work should be completed.
		/// </summary>
		protected IList<Action> OnCompletionActions { get; }
			= new List<Action>();

		/// <summary>
		/// Gets the list of actions to be invoked when the transaction should be rolled back.
		/// </summary>
		protected IList<Action> OnRollbackActions { get; }
			= new List<Action>();

		/// <summary>
		/// Gets the list of actions to be invoked when the transaction work should be executed.
		/// </summary>
		protected IList<Action> OnExecuteActions { get; }
			= new List<Action>();

		/// <summary>
		/// Adds a callback to be invoked during the completion phase.
		/// </summary>
		/// <param name="callback">
		/// The callback to be invoked.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="callback"/> is <c>null</c>.
		/// </exception>
		public void AddCompletionCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnCompletionActions.Add(callback);
		}

		/// <summary>
		/// Adds a callback to be invoked during the rollback phase.
		/// </summary>
		/// <param name="callback">
		/// The callback to be invoked.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="callback"/> is <c>null</c>.
		/// </exception>
		public void AddRollbackCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnRollbackActions.Add(callback);
		}

		/// <summary>
		/// Adds a callback to be invoked during the execution phase.
		/// </summary>
		/// <param name="callback">
		/// The callback to be invoked.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="callback"/> is <c>null</c>.
		/// </exception>
		public void AddExecuteCallback(Action callback)
		{
			Requires.NotNull(callback, nameof(callback));

			Requires.Argument(
				!InFinalPhase,
				nameof(callback),
				"Callbacks cannot be added when final phase of transaction has begun.");

			OnExecuteActions.Add(callback);
		}

		/// <summary>
		/// Must be called at least once when a transaction has begun so this transaction manager can enlist hinself.
		/// </summary>
		/// <returns>
		/// <c>true</c> if has, or already was, successfully enlisted in a transaction, else <c>false</c>.
		/// </returns>
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

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Commit(Enlistment)"/> callback.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="enlistment"/> is <c>null</c>.
		/// </exception>
		/// <see cref="AddCompletionCallback(Action)"/>
		protected virtual void OnCommitInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var completionAction in OnCompletionActions)
				completionAction();

			enlistment.Done();
		}

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.InDoubt(Enlistment)"/> callback.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="enlistment"/> is <c>null</c>.
		/// </exception>
		protected virtual void OnInDoubtInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			enlistment.Done();
		}

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Prepare(PreparingEnlistment)"/> callback. Invokes all execute-actions and notifies the system
		/// transaction manager that the transaction manager is ready to be committed. In case of any <see cref="Exception"/> during the execution of the execute-actions,
		/// will inform the underlying system transaction manager to force a rollback.
		/// </summary>
		/// <param name="preparingEnlistment">
		/// The underlying <see cref="PreparingEnlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="preparingEnlistment"/> is <c>null</c>.
		/// </exception>
		/// <seealso cref="AddExecuteCallback(Action)"/>
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

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Rollback(Enlistment)"/> callback. Executes all rollback actions.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="enlistment"/> is <c>null</c>.
		/// </exception>
		/// <seealso cref="AddRollbackCallback(Action)"/>
		protected virtual void OnRollbackInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			foreach (var rollbackAction in OnRollbackActions)
				rollbackAction();

			enlistment.Done();
		}

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Rollback(Enlistment)"/> callback.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="enlistment"/> is <c>null</c>.
		/// </exception>
		void IEnlistmentNotification.Commit(Enlistment enlistment)
		{
			OnCommitInternal(enlistment);
		}

		/// <summary>
		/// Dispatches to the <see cref="OnInDoubtInternal(Enlistment)"/> method.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/>.
		/// </param>
		void IEnlistmentNotification.InDoubt(Enlistment enlistment)
		{
			OnInDoubtInternal(enlistment);
		}

		/// <summary>
		/// Dispatches to the <see cref="OnPrepareInternal(PreparingEnlistment)"/> method.
		/// </summary>
		/// <param name="preparingEnlistment">
		/// The underlying <see cref="PreparingEnlistment"/>.
		/// </param>
		void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
		{
			OnPrepareInternal(preparingEnlistment);
		}

		/// <summary>
		/// Dispatches to the <see cref="OnRollbackInternal(Enlistment)"/> method.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/>.
		/// </param>
		void IEnlistmentNotification.Rollback(Enlistment enlistment)
		{
			OnRollbackInternal(enlistment);
		}
	}
}