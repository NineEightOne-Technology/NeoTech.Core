using System;
using System.Transactions;
using FluentAssertions;
using Moq;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenCommit : SinglePhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.Commit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldNotExecute()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					EnlistmentNotificationMock.Object,
					EnlistmentOptions.None);

				Sut.Commit(enlistment);
			}

			ExecuteActionMock.Verify(x => x(), Times.Never);
		}
	}
}