﻿using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;
using DataAnnotationsRange = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenUndoingAll : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldExecuteAllCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			var actionMock = AddSameCommand(numberOfCommandsToAdd);

			Sut.UndoAll();

			actionMock.Verify(x => x(), Times.Exactly(numberOfCommandsToAdd));
		}

		[Theory]
		[AutoData]
		public void ShouldClearAllCommands(
			[DataAnnotationsRange(10, 1000)] int numberOfCommandsToAdd)
		{
			var actionMock = AddSameCommand(numberOfCommandsToAdd);

			Sut.UndoAll();

			actionMock.Verify(x => x(), Times.Exactly(numberOfCommandsToAdd));

			Sut.Count
				.Should().Be(0);
		}
	}
}