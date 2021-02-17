//using FluentAssertions;
//using Moq;
//using System.Transactions;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
//{
//	public sealed class WhenCommitting : TransactionManagerTestBase
//	{
//		[Fact]
//		public void ShouldNotCommitWithoutCompleteTransctionScope()
//		{
//			AttachToCommit();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();
//			}

//			CommittingActionMock.Verify(action => action(), Times.Never);
//			CommitActionMock.Verify(action => action(), Times.Never);
//		}

//		[Fact]
//		public void ShouldCommitOnCompleteTransctionScope()
//		{
//			AttachToCommit();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();

//				transactionScope.Complete();
//			}

//			CommittingActionMock.Verify(action => action(), Times.Once);
//			CommitActionMock.Verify(action => action(), Times.Once);
//		}

//		[Fact]
//		public void ShouldNotThrowIfCommitActionsNotAttached()
//		{
//			using var transactionScope = new TransactionScope();

//			Sut.HookToCurrentTransaction();

//			transactionScope.Complete();
//		}

//		[Fact]
//		public void ShouldInvokeCommittingBeforeCommit()
//		{
//			AttachToCommit();

//			using (var transactionScope = new TransactionScope())
//			{
//				Sut.HookToCurrentTransaction();

//				transactionScope.Complete();
//			}

//			ExecutionOrder
//				.Should().HaveCount(2)
//				.And.ContainInOrder(
//					nameof(CommittingActionMock),
//					nameof(CommitActionMock));
//		}

//		[Fact]
//		public void ShouldNotCommitWithoutTransaction()
//		{
//			AttachToCommit();

//			Sut.HookToCurrentTransaction();

//			CommittingActionMock.Verify(action => action(), Times.Never);
//			CommitActionMock.Verify(action => action(), Times.Never);
//		}
//	}
//}