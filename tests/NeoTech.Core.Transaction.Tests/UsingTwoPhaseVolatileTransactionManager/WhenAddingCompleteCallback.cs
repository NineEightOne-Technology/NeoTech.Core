using System;
using FluentAssertions;
using NeoTech.Core.Transaction.Tests.Extensions;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhaseVolatileTransactionManager
{
	public sealed class WhenAddingCompleteCallback : TwoPhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnCallbackNull()
		{
			Sut
				.Invoking(x => x.AddCompletionCallback(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldThrowIfInFinalPhase()
		{
			Sut.SimulateFinalTransactionPhase();

			Sut
				.Invoking(x => x.AddCompletionCallback(CompleteActionMock.Object))
				.Should().ThrowExactly<ArgumentException>();
		}
	}
}
