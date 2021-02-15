using NeoTech.Transactions;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public abstract class StackCommandManagerTestBase
	{
		public StackCommandManagerTestBase()
		{
			Sut = new StackCommandManager();
		}

		protected StackCommandManager Sut { get; }
	}
}
