using Moq;
using NeoTech.Transactions;
using System.Transactions;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public abstract class EnlistmentNotificationTestBase
	{
		public EnlistmentNotificationTestBase()
		{
			EnlistmentNotificationMock = new Mock<IEnlistmentNotification>();
		}

		protected EnlistmentNotification Sut { get; }
			= new EnlistmentNotification();

		protected Mock<IEnlistmentNotification> EnlistmentNotificationMock { get; }
	}
}
