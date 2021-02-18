using System;
using System.Transactions;
using Moq;
using NeoTech.Core.Transactions;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public abstract class TransactionManagerTestBase<TTransactionManager, TEnlistmentNotification>
		where TTransactionManager : ITransactionManager, new()
		where TEnlistmentNotification : IEnlistmentNotification
	{
		public TransactionManagerTestBase()
		{
			ExecuteActionMock = new Mock<Action>();
			ExecuteActionMock.Setup(x => x()).Verifiable();

			CompleteActionMock = new Mock<Action>();
			CompleteActionMock.Setup(x => x()).Verifiable();

			RollbackActionMock = new Mock<Action>();
			RollbackActionMock.Setup(x => x()).Verifiable();

			Sut = new TTransactionManager();
			Sut.AddExecuteCallback(ExecuteActionMock.Object);
			Sut.AddCompletionCallback(CompleteActionMock.Object);
			Sut.AddRollbackCallback(RollbackActionMock.Object);
		}

		protected Mock<Action> ExecuteActionMock { get; }

		protected Mock<Action> CompleteActionMock { get; }

		protected Mock<Action> RollbackActionMock { get; }

		protected TTransactionManager Sut { get; }

		protected abstract TEnlistmentNotification SutAsEnlistmentNotification { get; }
	}
}