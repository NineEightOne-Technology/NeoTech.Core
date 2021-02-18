using System;
using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using FluentAssertions;
using NeoTech.Transaction.Tests.UsingStackCommandManager;
using Xunit;

namespace NeoTech.Core.Tests.UsingUndoCommandManager
{
	public sealed class WhenCounting : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		[InlineData(0)]
		public void ShouldCountNumberOfAddedCommands([Range(10, 1000)] int numberOfCommandsToAdd)
		{
			AddSameCommand(numberOfCommandsToAdd);

			Sut.Count
				.Should().Be(numberOfCommandsToAdd);
		}
	}
}
