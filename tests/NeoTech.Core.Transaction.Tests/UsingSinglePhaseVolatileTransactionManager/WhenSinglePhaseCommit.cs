using System;
using FluentAssertions;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenSinglePhaseCommit : SinglePhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			SutAsEnlistmentNotification
				.Invoking(x => x.SinglePhaseCommit(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}