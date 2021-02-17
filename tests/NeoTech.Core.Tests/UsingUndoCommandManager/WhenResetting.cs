using AutoFixture.Xunit2;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingStackCommandManager
{
	public sealed class WhenResetting : StackCommandManagerTestBase
	{
		[Theory]
		[AutoData]
		public void ShouldNotExecuteActionsAfterReset([Range(10, 100)] int numberOfActionsToInitializeWith)
		{
			var actionMocks = new List<Mock<Action>>();

			for (int i = 0; i < numberOfActionsToInitializeWith; i++)
			{
				var actionMock = new Mock<Action>();

				Sut.AddCommand(actionMock.Object);
			}

			Sut.Reset();

			using (new AssertionScope())
			{
				foreach (var mockedAction in actionMocks)
				{
					mockedAction.Verify(action => action(), Times.Never);
				}
			}
		}
	}
}