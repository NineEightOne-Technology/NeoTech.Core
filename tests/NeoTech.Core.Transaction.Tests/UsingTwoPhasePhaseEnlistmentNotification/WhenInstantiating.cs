using System;
using FluentAssertions;
using NeoTech.Core.Transactions;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification
{
	public sealed class WhenInstantiating : TwoPhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnParentNull()
		{
			Action ctorAction = () =>
			{
				new TwoPhaseEnlistmentNotification(null);
			};

			ctorAction
				.Invoking(x => x())
				.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}
