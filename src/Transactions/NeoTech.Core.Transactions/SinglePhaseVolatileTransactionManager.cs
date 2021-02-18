using System;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	/// <summary>
	/// Acts as a transaction manager that handes single phase commit transactions.
	/// </summary>
	public class SinglePhaseVolatileTransactionManager : TwoPhaseVolatileTransactionManager, ISinglePhaseNotification
	{
		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Prepare(PreparingEnlistment)"/> callback. Invokes all execute-actions and notifies the system
		/// transaction manager that the operation is completed. In case of any <see cref="Exception"/> during the execution of the execute-actions,
		/// will inform the underlying system transaction manager to force a rollback.
		/// </summary>
		/// <param name="preparingEnlistment">
		/// The underlying <see cref="PreparingEnlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="preparingEnlistment"/> is <c>null</c>.
		/// </exception>
		/// <seealso cref="TwoPhaseVolatileTransactionManager.AddExecuteCallback(Action)"/>
		protected override void OnPrepareInternal(PreparingEnlistment preparingEnlistment)
		{
			Requires.NotNull(preparingEnlistment, nameof(preparingEnlistment));

			InFinalPhase = true;

			try
			{
				foreach (var executeAction in OnExecuteActions)
					executeAction();

				preparingEnlistment.Done();
			}
			catch (Exception e)
			{
				preparingEnlistment.ForceRollback(e);
			}
		}

		/// <summary>
		/// Reacts to the <see cref="IEnlistmentNotification.Rollback(Enlistment)"/> callback. Sets the
		/// <see cref="TwoPhaseVolatileTransactionManager.InFinalPhase"/> to true and then delegates back to the base implementation.
		/// </summary>
		/// <param name="enlistment">
		/// The underlying <see cref="Enlistment"/> object.
		/// </param>
		/// <seealso cref="TwoPhaseVolatileTransactionManager.OnRollbackInternal(Enlistment)"/>.
		protected override void OnRollbackInternal(Enlistment enlistment)
		{
			InFinalPhase = true;

			base.OnRollbackInternal(enlistment);
		}

		/// <summary>
		/// Reacts to the <see cref="ISinglePhaseNotification.SinglePhaseCommit(SinglePhaseEnlistment)"/> callback. Invokes all execute-actions and
		/// notifies the system transaction manager that the operation is committed. In case of any <see cref="Exception"/> during the execution
		/// of the execute-actions, will inform the underlying system transaction manager that this commit has been aborted.
		/// </summary>
		/// <param name="singlePhaseEnlistment">
		/// The underlying <see cref="SinglePhaseEnlistment"/> object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="singlePhaseEnlistment"/> is <c>null</c>.
		/// </exception>
		protected virtual void OnSinglePhaseCommitInternal(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			Requires.NotNull(singlePhaseEnlistment, nameof(singlePhaseEnlistment));

			InFinalPhase = true;

			try
			{
				foreach (var executeAction in OnExecuteActions)
					executeAction();

				singlePhaseEnlistment.Committed();
			}
			catch (Exception ex)
			{
				singlePhaseEnlistment.Aborted(ex);
			}
		}

		/// <summary>
		/// Dispatches to <see cref="OnSinglePhaseCommitInternal(SinglePhaseEnlistment)"/>.
		/// </summary>
		/// <param name="singlePhaseEnlistment">
		/// The underlying <see cref="SinglePhaseEnlistment"/> object.
		/// </param>
		void ISinglePhaseNotification.SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			OnSinglePhaseCommitInternal(singlePhaseEnlistment);
		}
	}
}