using Moq;
using NeoTech.Core.Transactions;
using System;
using System.Transactions;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public abstract class EnlistmentNotificationTestBase<T>
		where T : IEnlistmentNotification
	{
		public EnlistmentNotificationTestBase()
		{
			ExecuteActionMock = new Mock<Action>();
			ExecuteActionMock.Setup(x => x()).Verifiable();

			CompleteActionMock = new Mock<Action>();
			CompleteActionMock.Setup(x => x()).Verifiable();

			RollbackActionMock = new Mock<Action>();
			RollbackActionMock.Setup(x => x()).Verifiable();

			TransactionManager = new Mock<ITransactionManager>();

			TransactionManager
				.SetupGet(x => x.OnExecuteActions)
				.Returns(new[] { ExecuteActionMock.Object });

			TransactionManager
				.SetupGet(x => x.OnCompletionActions)
				.Returns(new[] { CompleteActionMock.Object });

			TransactionManager
				.SetupGet(x => x.OnRollbackActions)
				.Returns(new[] { RollbackActionMock.Object });

			EnlistmentNotificationMock = new Mock<IEnlistmentNotification>();
		}

		protected Mock<Action> ExecuteActionMock { get; }

		protected Mock<Action> CompleteActionMock { get; }

		protected Mock<Action> RollbackActionMock { get; }

		protected abstract T Sut { get; }

		protected Mock<ITransactionManager> TransactionManager { get; }

		protected Mock<IEnlistmentNotification> EnlistmentNotificationMock { get; }
	}
}
