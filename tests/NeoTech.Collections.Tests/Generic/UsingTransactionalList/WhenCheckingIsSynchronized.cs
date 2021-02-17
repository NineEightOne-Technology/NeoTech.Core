using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenCheckingIsSynchronized : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldAlwaysReturnFalse()
		{
			Sut.IsSynchronized
				.Should().BeFalse();
		}
	}
}
