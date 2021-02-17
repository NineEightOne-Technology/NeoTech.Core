//using FluentAssertions;
//using Moq;
//using System;
//using System.Transactions;
//using Xunit;
//using SystemTransaction = System.Transactions.Transaction;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
//{
//	public sealed class WhenRollback : EnlistmentNotificationTestBase
//	{
//		[Fact]
//		public void ShouldThrowOnEnlistmentNull()
//		{
//			Sut
//				.Invoking(x => x.Rollback(null))
//				.Should().ThrowExactly<ArgumentNullException>();
//		}

//		[Fact]
//		public void ShouldInvokeCallbacks()
//		{
//			var onRollbackEventhandlerMock = new Mock<EventHandler>();
//			var onRollingBackEventhandlerMock = new Mock<EventHandler>();

//			Sut.OnRollback += onRollbackEventhandlerMock.Object;
//			Sut.OnRollingBack += onRollingBackEventhandlerMock.Object;

//			using (var transaction = new TransactionScope())
//			{
//				var enlistment = SystemTransaction.Current.EnlistVolatile(
//					EnlistmentNotificationMock.Object,
//					EnlistmentOptions.None);

//				Sut.Rollback(enlistment);
//			}

//			onRollbackEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
//			onRollingBackEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
//		}
//	}
//}