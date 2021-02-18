using System;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	public class SinglePhaseVolatileTransactionManager : TwoPhaseVolatileTransactionManager, ISinglePhaseNotification
	{
		protected override void OnCommitInternal(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			enlistment.Done();
		}

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

		protected override void OnRollbackInternal(Enlistment enlistment)
		{
			InFinalPhase = true;

			base.OnRollbackInternal(enlistment);
		}

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

		void ISinglePhaseNotification.SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			OnSinglePhaseCommitInternal(singlePhaseEnlistment);
		}
	}
}