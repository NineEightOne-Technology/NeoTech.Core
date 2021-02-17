using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenGettingSyncRoot : TransactionalQueueTestBase<string>
	{
		[Fact]
		public void ShouldReturnSyncObject()
		{
			Sut.SyncRoot
				.Should().NotBeNull();
		}
	}
}