using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenCheckingContains : TransactionalQueueTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldBeTrueIfItemIsContained([Range(0, MinNumberOfInitialItems - 1)] int randomIndex)
		{
			var randomItem = Sut.ElementAt(randomIndex);

			var result = Sut.Contains(randomItem);

			result
				.Should().BeTrue();
		}

		[Fact]
		public void ShouldBeFalseIfItemIsNotContained()
		{
			object item = Fixture.Create<string>();

			var result = Sut.Contains(item);

			result
				.Should().BeFalse();
		}
	}
}
