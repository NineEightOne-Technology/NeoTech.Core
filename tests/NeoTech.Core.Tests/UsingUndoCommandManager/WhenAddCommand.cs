using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenAddCommand : StackCommandManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnActionNull()
		{
			Sut
				.Invoking(x => x.AddCommand(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldAddCommandToExecutionStack()
		{
			var action = new Mock<Action>();

			Sut.AddCommand(action.Object);
			Sut.UndoAll();

			action.Verify(a => a(), Times.Once);
		}
	}
}