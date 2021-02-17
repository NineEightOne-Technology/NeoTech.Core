using System.Transactions;
using NeoTech.Core.Transactions;
using NeoTech.Transaction.Tests.UsingEnlistmentNotification;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public abstract class SinglePhaseEnlistmentNotificationTestBase : EnlistmentNotificationTestBase<ISinglePhaseNotification>
	{
		public SinglePhaseEnlistmentNotificationTestBase()
		{
			Sut = new SinglePhaseEnlistmentNotification(
				base.TransactionManager.Object);
		}

		protected override ISinglePhaseNotification Sut { get; }
	}
}