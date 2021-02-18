﻿using System;
using FluentAssertions;
using NeoTech.Core.Transaction.Tests.Extensions;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhaseVolatileTransactionManager
{
	public sealed class WhenAddingExecuteCallback : TwoPhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnCallbackNull()
		{
			Sut
				.Invoking(x => x.AddExecuteCallback(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldThrowIfInFinalPhase()
		{
			Sut.SimulateFinalTransactionPhase();

			Sut
				.Invoking(x => x.AddExecuteCallback(ExecuteActionMock.Object))
				.Should().ThrowExactly<ArgumentException>();
		}
	}
}
