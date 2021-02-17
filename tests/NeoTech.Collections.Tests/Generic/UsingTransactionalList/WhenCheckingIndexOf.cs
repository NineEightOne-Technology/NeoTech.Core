using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenCheckingIndexOf : TransactionalListTestBase<string>
	{
		[Fact]
		public void ShouldReturnMinusOneForUnknownItem()
		{
			object item = Fixture.Create<string>();

			var result = SutWithObjects.IndexOf(item);

			result
				.Should().Be(-1);
		}

		[Theory]
		[AutoData]
		public void ShouldReturnCorrectIndexForKnownItem(
			string item,
			[Range(0, MinNumberOfInitialItems - 1)] int randomInsertIndex)
		{
			Sut.Insert(randomInsertIndex, item);

			var result = SutWithObjects.IndexOf(item);

			result
				.Should().Be(randomInsertIndex);
		}
	}
}