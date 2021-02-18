using System;
using System.Transactions;
using FluentAssertions;
using Moq;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenCommit : SinglePhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			SutAsEnlistmentNotification
				.Invoking(x => x.Commit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldNotExecute()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					SutAsEnlistmentNotification,
					EnlistmentOptions.None);

				SutAsEnlistmentNotification.Commit(enlistment);
			}

			ExecuteActionMock.Verify(x => x(), Times.Never);
		}

		[Fact]
		public void ShouldComplete()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					SutAsEnlistmentNotification,
					EnlistmentOptions.None);

				SutAsEnlistmentNotification.Commit(enlistment);
			}

			CompleteActionMock.Verify(x => x(), Times.Once);
		}
	}
}