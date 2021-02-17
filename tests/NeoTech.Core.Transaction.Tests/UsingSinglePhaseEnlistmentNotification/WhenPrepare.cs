using System;
using FluentAssertions;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenPrepare : SinglePhaseEnlistmentNotificationTestBase
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