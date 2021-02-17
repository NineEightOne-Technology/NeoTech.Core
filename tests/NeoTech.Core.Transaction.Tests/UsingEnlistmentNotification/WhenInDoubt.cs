//using FluentAssertions;
//using Moq;
//using System;
//using System.Transactions;
//using Xunit;
//using SystemTransaction = System.Transactions.Transaction;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
//{
//	public sealed class WhenInDoubt : EnlistmentNotificationTestBase
//	{
//		[Fact]
//		public void ShouldThrowOnEnlistmentNull()
//		{
//			Sut
//				.Invoking(x => x.InDoubt(null))
//				.Should().ThrowExactly<ArgumentNullException>();
//		}

//		[Fact]
//		public void ShouldInvokeCallbacks()
//		{
//			var onDoubtEventhandlerMock = new Mock<EventHandler>();
//			var onDoubtingEventhandlerMock = new Mock<EventHandler>();

//			Sut.OnDoubted += onDoubtEventhandlerMock.Object;
//			Sut.OnDoubting += onDoubtingEventhandlerMock.Object;

//			using (var transaction = new TransactionScope())
//			{
//				var enlistment = SystemTransaction.Current.EnlistVolatile(
//					EnlistmentNotificationMock.Object,
//					EnlistmentOptions.None);

//				Sut.InDoubt(enlistment);
//			}

//			onDoubtEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
//			onDoubtingEventhandlerMock.Verify(x => x(Sut, EventArgs.Empty), Times.Once);
//		}

//		[Fact]
//		public void ShouldInvokeRollback()
//		{
//			using var transaction = new TransactionScope();

//			var enlistment = SystemTransaction.Current.EnlistVolatile(
//				EnlistmentNotificationMock.Object,
//				EnlistmentOptions.None);

//			Sut.InDoubt(enlistment);
//		}
//	}
//}