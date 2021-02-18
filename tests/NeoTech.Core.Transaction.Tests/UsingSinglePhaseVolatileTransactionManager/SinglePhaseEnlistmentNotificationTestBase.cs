using System.Transactions;
using NeoTech.Core.Transactions;
using NeoTech.Transaction.Tests.UsingEnlistmentNotification;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public abstract class SinglePhaseVolatileTransactionManagerTestBase
		: TransactionManagerTestBase<SinglePhaseVolatileTransactionManager, ISinglePhaseNotification>
	{
		protected override ISinglePhaseNotification SutAsEnlistmentNotification => Sut;
	}
}