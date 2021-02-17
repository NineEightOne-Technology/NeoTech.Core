using System;
using System.Transactions;
using FluentAssertions;
using Moq;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public sealed class WhenRollback : TwoPhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.Rollback(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldExecuteRollback()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					EnlistmentNotificationMock.Object,
					EnlistmentOptions.None);

				Sut.Rollback(enlistment);
			}

			RollbackActionMock.Verify(x => x(), Times.Once);
		}
	}
}