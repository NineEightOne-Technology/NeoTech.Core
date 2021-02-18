using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NeoTech.Transaction.Tests.UsingStackCommandManager;
using Xunit;
using DataAnnotationsRange = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace NeoTech.Core.Tests.UsingUndoCommandManager
{
	public sealed class WhenResetting : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldClearListOfCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			AddSameCommand(numberOfCommandsToAdd);

			Sut.Count
				.Should().Be(numberOfCommandsToAdd);

			Sut.Reset();

			Sut.Count
				.Should().Be(0);
		}

		[Theory]
		[AutoData]
		public void ShouldNotExecute(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			var actionMock = AddSameCommand(numberOfCommandsToAdd);

			Sut.Reset();

			actionMock.Verify(x => x(), Times.Never);
		}
	}
}