//using FluentAssertions;
//using Moq;
//using System.Transactions;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
//{
//	public sealed class WhenRollingBack : TransactionManagerTestBase
//	{
//		[Fact]
//		public void ShouldRollbackWithoutCompleteTransctionScope()
//		{
//			AttachToRollback();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();
//			}

//			RollingBackActionMock.Verify(action => action(), Times.Once);
//			RollbackActionMock.Verify(action => action(), Times.Once);
//		}

//		[Fact]
//		public void ShouldNotRollbackOnCompleteTransctionScope()
//		{
//			AttachToRollback();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();

//				transactionScope.Complete();
//			}

//			RollingBackActionMock.Verify(action => action(), Times.Never);
//			RollbackActionMock.Verify(action => action(), Times.Never);
//		}

//		[Fact]
//		public void ShouldInvokeRollingBackBeforeRollbackOnNonCompleteTransactionScope()
//		{
//			AttachToRollback();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();
//			}

//			ExecutionOrder
//				.Should().HaveCount(2)
//				.And.ContainInOrder(
//					nameof(RollingBackActionMock),
//					nameof(RollbackActionMock));
//		}

//		[Fact]
//		public void ShouldNotThrowIfRollbackActionsNotAttached()
//		{
//			using var transactionScope = new TransactionScope();

//			Sut.HookToCurrentTransaction();
//		}

//		[Fact]
//		public void ShouldNotRollbackWithoutTransaction()
//		{
//			AttachToRollback();

//			Sut.HookToCurrentTransaction();

//			RollingBackActionMock.Verify(action => action(), Times.Never);
//			RollbackActionMock.Verify(action => action(), Times.Never);
//		}
//	}
//}