using System.Transactions;

namespace NeoTech.Core.Transactions
{
	public class SinglePhaseVolatileTransactionManager : TwoPhaseVolatileTransactionManager
	{
		protected override IEnlistmentNotification GetEnlistmentNotification()
		{
			return new SinglePhaseEnlistmentNotification(this);
		}
	}
}
