using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenCheckingContains : TransactionalListTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldBeTrueIfItemIsContained([Range(0, MinNumberOfInitialItems - 1)] int randomIndex)
		{
			var randomItem = SutWithObjects[randomIndex];

			var result = SutWithObjects.Contains(randomItem);

			result
				.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeFalseIfItemIsNotContained()
		{
			object item = Fixture.Create<string>();

			var result = SutWithObjects.Contains(item);

			result
				.Should().BeFalse();
		}
	}
}
