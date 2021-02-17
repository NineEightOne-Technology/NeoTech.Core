using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenGettingEnumerator : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldReturnEnumeratorWithExactItems()
		{
			var enumerator = SutWithObjects.GetEnumerator();

			List<object> result = new List<object>();

			while (enumerator.MoveNext())
			{
				result.Add(enumerator.Current);
			}

			result
				.Should().BeEquivalentTo(Sut, o => o.WithStrictOrdering());
		}
	}
}