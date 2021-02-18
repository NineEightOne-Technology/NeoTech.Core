using System;
using System.Transactions;
using FluentAssertions;
using Moq;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhaseVolatileTransactionManager
{
	public sealed class WhenRollback : TwoPhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			SutAsEnlistmentNotification
				.Invoking(x => x.Rollback(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldExecuteRollback()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					Sut,
					EnlistmentOptions.None);

				SutAsEnlistmentNotification.Rollback(enlistment);
			}

			RollbackActionMock.Verify(x => x(), Times.Once);
		}
	}
}