using Moq;
using NeoTech.Core.Transactions;
using System;
using System.Collections.Generic;
using SystemTransaction = System.Transactions.Transaction;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
{
	public abstract class TransactionManagerTestBase
	{
		public TransactionManagerTestBase()
		{
			Sut = new TransactionManager();
		}

		protected TransactionManager Sut { get; }

		protected List<string> ExecutionOrder { get; }
		= new List<string>();

		protected Mock<Action> CommitActionMock { get; private set; }

		protected Mock<Action> CommittingActionMock { get; private set; }

		protected Mock<Action> RollbackActionMock { get; private set; }

		protected Mock<Action> RollingBackActionMock { get; private set; }

		protected Mock<Action> PreparedActionMock { get; private set; }

		protected Mock<Action> PreparingBackActionMock { get; private set; }

		protected Mock<Action<SystemTransaction>> TransactionCompletedActionMock { get; private set; }

		protected void AttachToCommit()
		{
			CommittingActionMock = new Mock<Action>();
			CommittingActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(CommittingActionMock)));

			CommitActionMock = new Mock<Action>();
			CommitActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(CommitActionMock)));

			Sut.OnCommittingAction = CommittingActionMock.Object;
			Sut.OnCommitAction = CommitActionMock.Object;
		}

		protected void AttachToRollback()
		{
			RollingBackActionMock = new Mock<Action>();
			RollingBackActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(RollingBackActionMock)));

			RollbackActionMock = new Mock<Action>();
			RollbackActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(RollbackActionMock)));

			Sut.OnRollingBackAction = RollingBackActionMock.Object;
			Sut.OnRollbackAction = RollbackActionMock.Object;
		}

		protected void AttachToPrepare()
		{
			PreparingBackActionMock = new Mock<Action>();
			PreparingBackActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(PreparingBackActionMock)));

			PreparedActionMock = new Mock<Action>();
			PreparedActionMock
				.Setup(x => x())
				.Callback(() => ExecutionOrder.Add(nameof(PreparedActionMock)));

			Sut.OnPreparingAction = PreparingBackActionMock.Object;
			Sut.OnPreparedAction = PreparedActionMock.Object;
		}

		protected void AttachToTransactionCompleted()
		{
			TransactionCompletedActionMock = new Mock<Action<SystemTransaction>>();
			TransactionCompletedActionMock
				.Setup(x => x(It.IsAny<SystemTransaction>()))
				.Verifiable();

			Sut.OnTransactionCompletedAction = TransactionCompletedActionMock.Object;
		}
	}
}