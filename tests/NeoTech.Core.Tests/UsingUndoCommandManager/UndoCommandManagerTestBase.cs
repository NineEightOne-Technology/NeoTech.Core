using NeoTech.Core.Command;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public abstract class StackCommandManagerTestBase
	{
		public StackCommandManagerTestBase()
		{
			Sut = new UndoCommandManager();
		}

		protected UndoCommandManager Sut { get; }
	}
}
