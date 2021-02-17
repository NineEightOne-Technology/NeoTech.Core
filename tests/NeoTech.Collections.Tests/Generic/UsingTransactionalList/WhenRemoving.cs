using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenRemoving : TransactionalListTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldRemoveWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfRemovals)
		{
			for (int i = 0; i < numberOfRemovals; i++)
			{
				var removalIndex = RandomNumberGenerator.Next(0, Sut.Count);

				var oldItem = SutWithObjects[removalIndex];

				Sut.Remove(oldItem);

				Sut
					.Should().NotContain((string)oldItem);
			}

			Sut
				.Should().HaveCount(InitialItems.Count - numberOfRemovals);
		}

		[Theory]
		[AutoData]
		public void ShouldRemoveOnCommit([Range(1, MinNumberOfInitialItems)] int numberOfRemovals)
		{
			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfRemovals; i++)
				{
					var removalIndex = RandomNumberGenerator.Next(0, Sut.Count);

					var oldItem = SutWithObjects[removalIndex];

					Sut.Remove(oldItem);

					Sut
						.Should().NotContain((string)oldItem);
				}

				transaction.Complete();
			}

			Sut
				.Should().HaveCount(InitialItems.Count - numberOfRemovals);
		}

		[Theory]
		[AutoData]
		public void ShouldNotRemoveOnRollback([Range(1, MinNumberOfInitialItems)] int numberOfRemovals)
		{
			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfRemovals; i++)
				{
					var removalIndex = RandomNumberGenerator.Next(0, Sut.Count);

					var oldItem = SutWithObjects[removalIndex];

					Sut.Remove(oldItem);

					Sut
						.Should().NotContain((string)oldItem);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}