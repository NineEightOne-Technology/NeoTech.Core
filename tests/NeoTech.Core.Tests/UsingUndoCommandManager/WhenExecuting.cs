using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NeoTech.Core.Abstractions.Command;
using NeoTech.Transaction.Tests.UsingStackCommandManager;
using Xunit;
using DataAnnotationsRange = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace NeoTech.Core.Tests.UsingUndoCommandManager
{
	public sealed class WhenExecuting : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldExecuteAllCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			var actionMock = AddSameCommand(numberOfCommandsToAdd);

			((ICommandManager)Sut).Execute();

			actionMock.Verify(x => x(), Times.Exactly(numberOfCommandsToAdd));
		}

		[Theory]
		[AutoData]
		public void ShouldClearAllCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			var actionMock = AddSameCommand(numberOfCommandsToAdd);

			((ICommandManager)Sut).Execute();

			actionMock.Verify(x => x(), Times.Exactly(numberOfCommandsToAdd));

			Sut.Count
				.Should().Be(0);
		}
	}
}