using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;
using DataAnnotationsRange = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenUndoingSteps : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldThrowIfNumberOfStepsToUndoGreaterThanAddedCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			AddSameCommand(numberOfCommandsToAdd);

			Sut
				.Invoking(x => x.Undo(numberOfCommandsToAdd + 1))
				.Should().ThrowExactly<ArgumentException>();

		}

		[Theory]
		[AutoData]
		public void ShouldExecuteNumberOfCommandsSpecified(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd,
			[DataAnnotationsRange(1, 10)] int numberOfCommandsToUndo)
		{
			var actionMock =  AddSameCommand(numberOfCommandsToAdd);

			Sut.Undo(numberOfCommandsToUndo);

			actionMock.Verify(x => x(), Times.Exactly(numberOfCommandsToUndo));
		}

		[Theory]
		[AutoData]
		public void ShouldKeepNonExecutedCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd,
			[DataAnnotationsRange(1, 10)] int numberOfCommandsToUndo)
		{
			AddSameCommand(numberOfCommandsToAdd);

			Sut.Undo(numberOfCommandsToUndo);

			Sut.Count
				.Should().Be(numberOfCommandsToAdd - numberOfCommandsToUndo);
		}
	}
}