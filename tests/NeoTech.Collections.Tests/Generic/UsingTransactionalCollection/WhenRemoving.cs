using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public sealed class WhenRemoving : TransactionalCollectionTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldRemoveWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfRemovals)
		{
			for (int i = 0; i < numberOfRemovals; i++)
			{
				var removalIndex = RandomNumberGenerator.Next(0, Sut.Count);

				var oldItem = Sut[removalIndex];

				var itemRemovalResult = Sut.Remove(oldItem);

				itemRemovalResult
					.Should().BeTrue();

				Sut
					.Should().NotContain(oldItem);
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

					var oldItem = Sut[removalIndex];

					var itemRemovalResult = Sut.Remove(oldItem);

					itemRemovalResult
						.Should().BeTrue();

					Sut
						.Should().NotContain(oldItem);
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

					var oldItem = Sut[removalIndex];

					var itemRemovalResult = Sut.Remove(oldItem);

					itemRemovalResult
						.Should().BeTrue();

					Sut
						.Should().NotContain(oldItem);
				}
			}

			Sut
				.Should().HaveSameCount(InitialItems);
		}
	}
}