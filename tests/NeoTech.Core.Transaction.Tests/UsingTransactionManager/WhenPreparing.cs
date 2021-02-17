//using FluentAssertions;
//using Moq;
//using System.Transactions;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
//{
//	public sealed class WhenPreparing : TransactionManagerTestBase
//	{
//		[Fact]
//		public void ShouldNotPrepareWithoutCompleteTransctionScope()
//		{
//			AttachToPrepare();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();
//			}

//			PreparingBackActionMock.Verify(action => action(), Times.Never);
//			PreparedActionMock.Verify(action => action(), Times.Never);
//		}

//		[Fact]
//		public void ShouldPrepareOnCompleteTransctionScope()
//		{
//			AttachToPrepare();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();

//				transactionScope.Complete();
//			}

//			PreparingBackActionMock.Verify(action => action(), Times.Once);
//			PreparedActionMock.Verify(action => action(), Times.Once);
//		}

//		[Fact]
//		public void ShouldInvokePreparingBeforePrepareOnCompleteTransactionScope()
//		{
//			AttachToPrepare();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();

//				transactionScope.Complete();
//			}

//			ExecutionOrder
//				.Should().HaveCount(2)
//				.And.ContainInOrder(
//					nameof(PreparingBackActionMock),
//					nameof(PreparedActionMock));
//		}

//		[Fact]
//		public void ShouldNotThrowIfPrepareActionsNotAttached()
//		{
//			using var transactionScope = new TransactionScope();

//			Sut.HookToCurrentTransaction();
//		}

//		[Fact]
//		public void ShouldNotPrepareWithoutTransaction()
//		{
//			AttachToPrepare();

//			Sut.HookToCurrentTransaction();

//			PreparingBackActionMock.Verify(action => action(), Times.Never);
//			PreparedActionMock.Verify(action => action(), Times.Never);
//		}
//	}
//}