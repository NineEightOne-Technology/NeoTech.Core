using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenAddCommand : UndoCommandManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnActionNull()
		{
			Sut
				.Invoking(x => x.AddCommand(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Theory]
		[AutoData]
		public void ShouldThrowIfCommandCountLargerThanMemory([Range(10, 1000)] int numberOfCommandsToAdd)
		{
			Options.CommandMemory = numberOfCommandsToAdd - 1;

			Sut
				.Invoking(x =>
				{
					AddSameCommand(numberOfCommandsToAdd);
				})
				.Should().ThrowExactly<InvalidOperationException>();
		}

		[Theory]
		[AutoData]
		public void ShouldAddCommandToExecutionStack([Range(10, 1000)] int numberOfCommandsToAdd)
		{
			AddSameCommand(numberOfCommandsToAdd);

			Sut.Count
				.Should().Be(numberOfCommandsToAdd);
		}
	}
}