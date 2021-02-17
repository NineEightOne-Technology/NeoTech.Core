using System;
using FluentAssertions;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenSinglePhaseCommit : SinglePhaseEnlistmentNotificationTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			Sut
				.Invoking(x => x.SinglePhaseCommit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}