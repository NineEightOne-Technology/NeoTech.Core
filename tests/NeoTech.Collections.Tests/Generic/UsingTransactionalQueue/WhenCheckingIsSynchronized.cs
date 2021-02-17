using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenCheckingIsSynchronized : TransactionalQueueTestBase<string>
	{
		[Fact]
		public void ShouldAlwaysReturnFalse()
		{
			Sut.IsSynchronized
				.Should().BeFalse();
		}
	}
}
