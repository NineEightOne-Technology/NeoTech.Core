using System.Transactions;
using NeoTech.Core.Transactions;
using NeoTech.Transaction.Tests.UsingEnlistmentNotification;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification
{
	public abstract class TwoPhaseEnlistmentNotificationTestBase : EnlistmentNotificationTestBase<IEnlistmentNotification>
	{
		public TwoPhaseEnlistmentNotificationTestBase()
		{
			Sut = new TwoPhaseEnlistmentNotification(
				TransactionManager.Object);
		}

		protected override IEnlistmentNotification Sut { get; }
	}
}