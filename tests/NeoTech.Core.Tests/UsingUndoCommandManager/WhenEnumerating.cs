using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NeoTech.Transaction.Tests.UsingStackCommandManager;
using Xunit;
using DataAnnotationsRange = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace NeoTech.Core.Tests.UsingUndoCommandManager
{
	public sealed class WhenEnumerating : UndoCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldIterateOverAddedCommands(
			[DataAnnotationsRange(10, 100)] int numberOfCommandsToAdd)
		{
			var addedActionList = new List<Action>();
			var enumeratedActionList = new List<Action>();

			for (int i = 0; i < numberOfCommandsToAdd; i++)
			{
				var action = new Mock<Action>();

				Sut.AddCommand(action.Object);

				addedActionList.Add(action.Object);
			}

			var enumerator = Sut.GetEnumerator();

			while (enumerator.MoveNext())
			{
				enumeratedActionList.Add(enumerator.Current);
			}

			enumeratedActionList
				.Should().HaveSameCount(addedActionList)
				.And.BeEquivalentTo(addedActionList);
		}

		[Theory]
		[AutoData]
		public void ShouldIterateOverAddedCommandsOnObjectEnumerator(
			[DataAnnotationsRange(10, 100)] int numberOfCommandsToAdd)
		{
			var addedActionList = new List<Action>();
			var enumeratedActionList = new List<Action>();

			for (int i = 0; i < numberOfCommandsToAdd; i++)
			{
				var action = new Mock<Action>();

				Sut.AddCommand(action.Object);

				addedActionList.Add(action.Object);
			}

			var enumerator = ((IEnumerable)Sut).GetEnumerator();

			while (enumerator.MoveNext())
			{
				enumerator.Current
					.Should().BeOfType<Action>();

				enumeratedActionList.Add((Action)enumerator.Current);
			}

			enumeratedActionList
				.Should().HaveSameCount(addedActionList)
				.And.BeEquivalentTo(addedActionList);
		}
	}
}