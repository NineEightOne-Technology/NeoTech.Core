using FluentAssertions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenCheckingIsReadOnly : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldAlwaysReturnFalse()
		{
			Sut.IsReadOnly
				.Should().BeFalse();
		}
	}
}
