using FluentAssertions;
using Moq;
using System;
using System.Transactions;
using Xunit;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public sealed class WhenCommit : EnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.Commit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldInvokeCallbacks()
		{
			var onCommitEventhandlerMock = new Mock<EventHandler>();
			var onCommittingEventhandlerMock = new Mock<EventHandler>();

			Sut.OnCommit += onCommitEventhandlerMock.Object;
			Sut.OnCommitting += onCommittingEventhandlerMock.Object;

			using (var transaction = new TransactionScope())
			{
				var enlistment = SystemTransaction.Current.EnlistVolatile(
					EnlistmentNotificationMock.Object,
					EnlistmentOptions.None);

				Sut.Commit(enlistment);
			}

			onCommitEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
			onCommittingEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
		}
	}
}