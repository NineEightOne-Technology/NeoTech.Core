using Moq;
using NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager;
using System.Transactions;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Transaction.Tests.UsingTransactionManager
{
	public sealed class WhenCompletingTransaction : TransactionManagerTestBase
	{
		[Fact]
		public void ShouldNotInvokeWithoutTransaction()
		{
			AttachToTransactionCompleted();

			Sut.HookToCurrentTransaction();

			TransactionCompletedActionMock.Verify(x => x(It.IsAny<SystemTransaction>()), Times.Never);
		}

		[Fact]
		public void ShouldInvokeOnCommit()
		{
			AttachToTransactionCompleted();

			using(var transaction = new TransactionScope())
			{
				Sut.HookToCurrentTransaction();

				transaction.Complete();
			}

			TransactionCompletedActionMock.Verify(x => x(It.IsAny<SystemTransaction>()), Times.Once);
		}

		[Fact]
		public void ShouldInvokeOnRollback()
		{
			AttachToTransactionCompleted();

			using (var transaction = new TransactionScope())
			{
				Sut.HookToCurrentTransaction();
			}

			TransactionCompletedActionMock.Verify(x => x(It.IsAny<SystemTransaction>()), Times.Once);
		}
	}
}