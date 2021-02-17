using System;
using FluentAssertions;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
{
	public sealed class WhenPrepare : TwoPhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.Prepare(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}