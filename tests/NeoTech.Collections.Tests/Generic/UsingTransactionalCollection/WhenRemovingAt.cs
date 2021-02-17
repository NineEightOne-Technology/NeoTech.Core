using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public sealed class WhenRemovingAt : TransactionalCollectionTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldRemoveAtWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfRemovals)
		{
			for (int i = 0; i < numberOfRemovals; i++)
			{
				var removalIndex = RandomNumberGenerator.Next(0, Sut.Count);

				Sut.RemoveAt(removalIndex);
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

					Sut.RemoveAt(removalIndex);
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

					Sut.RemoveAt(removalIndex);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}