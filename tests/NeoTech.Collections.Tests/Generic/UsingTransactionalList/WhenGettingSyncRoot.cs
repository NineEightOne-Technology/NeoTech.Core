using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenGettingSyncRoot : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldReturnSyncObject()
		{
			var syncRoot = Sut.SyncRoot;

			syncRoot
				.Should().NotBeNull();
		}
	}
}