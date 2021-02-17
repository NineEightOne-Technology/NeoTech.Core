using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenPeeking : TransactionalQueueTestBase<string>
	{
		[Fact]
		public void ShouldAlwaysReturnNextInQueue()
		{
			var peekedItem = Sut.Peek();

			var dequeuedItem = Sut.Dequeue();

			peekedItem
				.Should().Be(dequeuedItem);
		}
	}
}