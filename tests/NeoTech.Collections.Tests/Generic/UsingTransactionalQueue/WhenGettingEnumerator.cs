using FluentAssertions;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenGettingEnumerator : TransactionalQueueTestBase<string>
	{
		[Fact]
		public void ShouldReturnEnumeratorWithExactItems()
		{
			var enumerator = ((ICollection)Sut).GetEnumerator();

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