using System.Transactions;
using NeoTech.Core.Transactions;
using NeoTech.Transaction.Tests.UsingEnlistmentNotification;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification
{
	public abstract class TwoPhaseVolatileTransactionManagerTestBase
		: TransactionManagerTestBase<TwoPhaseVolatileTransactionManager, IEnlistmentNotification>
	{
		protected override IEnlistmentNotification SutAsEnlistmentNotification => Sut;
	}
}