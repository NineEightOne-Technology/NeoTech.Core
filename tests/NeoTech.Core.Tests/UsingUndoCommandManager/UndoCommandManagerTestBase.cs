using System;
using Moq;
using NeoTech.Core.Command;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public abstract class UndoCommandManagerTestBase
	{
		public UndoCommandManagerTestBase()
		{
			Sut = new UndoCommandManager(Options);
		}

		protected UndoCommandManager Sut { get; }

		protected UndoCommandManagerOptions Options
			= new UndoCommandManagerOptions();

		protected Mock<Action> AddSameCommand(int numberOfTimes)
		{
			var actionMock = new Mock<Action>();
			actionMock.Setup(x => x()).Verifiable();

			for (int i = 0; i < numberOfTimes; i++)
			{
				Sut.AddCommand(actionMock.Object);
			}

			return actionMock;
		}
	}
}
