using NeoTech.Core.Abstractions.Command;
using NeoTech.Core.Command;

namespace NeoTech.Core.Tests.Command.UsingUndoCommandManager
{
	public abstract class UndoCommandManagerTestBase
	{
		public UndoCommandManagerTestBase()
		{
			Sut = new UndoCommandManager();
		}

		protected ICommandManager Sut { get; }
	}
}