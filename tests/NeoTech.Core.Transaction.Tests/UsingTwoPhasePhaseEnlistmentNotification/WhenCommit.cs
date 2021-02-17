using System;
using System.Transactions;
using FluentAssertions;
using Moq;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public sealed class WhenCommit : TwoPhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.Commit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldComplete()
		{
			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					EnlistmentNotificationMock.Object,
					EnlistmentOptions.None);

				Sut.Commit(enlistment);
			}

			CompleteActionMock.Verify(x => x(), Times.Once);
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