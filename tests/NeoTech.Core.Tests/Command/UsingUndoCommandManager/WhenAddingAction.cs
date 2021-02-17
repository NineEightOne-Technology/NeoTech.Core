using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace NeoTech.Core.Tests.Command.UsingUndoCommandManager
{
	public sealed class WhenAddingAction : UndoCommandManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnActionNull()
		{
			Sut
				.Invoking(x => x.AddCommand(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldAddActionToExecutionStack()
		{
			var action = new Mock<Action>();

			Sut.AddCommand(action.Object);
			Sut.Execute();

			action.Verify(a => a(), Times.Once);
		}
	}
}