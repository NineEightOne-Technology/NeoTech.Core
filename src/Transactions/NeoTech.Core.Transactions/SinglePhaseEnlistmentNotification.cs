using System;
using System.Transactions;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	internal sealed class SinglePhaseEnlistmentNotification : TwoPhaseEnlistmentNotification, ISinglePhaseNotification
	{
		private readonly ITransactionManager _parent;

		public SinglePhaseEnlistmentNotification(ITransactionManager parent)
			: base(parent) { }

		public override void Commit(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			enlistment.Done();
		}

		public override void Prepare(PreparingEnlistment preparingEnlistment)
		{
			Requires.NotNull(preparingEnlistment, nameof(preparingEnlistment));

			try
			{
				foreach (var executeAction in _parent.OnExecuteActions)
					executeAction();

				preparingEnlistment.Done();
			}
			catch (Exception e)
			{
				preparingEnlistment.ForceRollback(e);
			}
		}

		public void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
		{
			Requires.NotNull(singlePhaseEnlistment, nameof(singlePhaseEnlistment));

			try
			{
				foreach (var executeAction in _parent.OnExecuteActions)
					executeAction();

				singlePhaseEnlistment.Committed();
			}
			catch (Exception ex)
			{
				singlePhaseEnlistment.Aborted(ex);
			}
		}
	}
}
