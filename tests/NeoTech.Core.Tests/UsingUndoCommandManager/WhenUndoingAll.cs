using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenUndoingAll : StackCommandManagerTestBase
	{
		private readonly List<Mock<Action>> _mockedActions;
		private readonly List<int> _actionInvocationIndexes;

		public WhenUndoingAll()
		{
			_mockedActions = new List<Mock<Action>>();
			_actionInvocationIndexes = new List<int>();

			var fixture = new Fixture();
			var numberOfActionInstances = fixture.Create<int>();

			for (int i = 0; i < numberOfActionInstances; i++)
			{
				var scopedIndex = i;

				var actionMock = new Mock<Action>();

				actionMock
					.Setup(a => a())
					.Callback(() => _actionInvocationIndexes.Add(scopedIndex));

				_mockedActions.Add(actionMock);

				Sut.AddCommand(actionMock.Object);
			}
		}

		[Fact]
		public void ShouldExecuteEachAction()
		{
			Sut.UndoAll();

			foreach (var mockedAction in _mockedActions)
			{
				mockedAction.Verify(action => action(), Times.Once);
			}
		}

		[Theory]
		[AutoData]
		public void ShouldExecuteEachActionOnlyOnce([Range(10, 1000)] int numberOfExecutions)
		{
			for (int i = 0; i < numberOfExecutions; i++)
				Sut.UndoAll();

			foreach (var mockedAction in _mockedActions)
			{
				mockedAction.Verify(action => action(), Times.Once);
			}

		}

		[Fact]
		public void ShouldExecuteActionInLiFoOrder()
		{
			Sut.UndoAll();

			_actionInvocationIndexes
				.Should().BeInDescendingOrder();
		}
	}
}