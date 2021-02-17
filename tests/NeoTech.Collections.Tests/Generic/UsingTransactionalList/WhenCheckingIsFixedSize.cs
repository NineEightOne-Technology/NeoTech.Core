using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenCheckingIsFixedSize : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldAlwaysReturnFalse()
		{
			Sut.IsFixedSize
				.Should().BeFalse();
		}
	}
}
