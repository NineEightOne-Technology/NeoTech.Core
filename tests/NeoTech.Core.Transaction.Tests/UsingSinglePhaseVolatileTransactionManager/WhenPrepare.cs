using System;
using FluentAssertions;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingSinglePhaseEnlistmentNotification
{
	public sealed class WhenPrepare : SinglePhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnEnlistmentNull()
		{
			SutAsEnlistmentNotification
				.Invoking(x => x.Prepare(null))
				.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}