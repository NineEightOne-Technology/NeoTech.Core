using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public sealed class WhenAddingItem : TransactionalCollectionTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldAddWithoutTransaction(string[] itemsToAdd)
		{
			foreach (var item in itemsToAdd)
			{
				Sut.Add(item);
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToAdd.Length);

			Sut
				.Should().Contain(itemsToAdd);
		}

		[Theory]
		[AutoData]
		public void ShouldAddOnCommit(string[] itemsToAdd)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToAdd)
				{
					Sut.Add(item);
				}

				transactionScope.Complete();
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToAdd.Length);

			Sut
				.Should().Contain(itemsToAdd);
		}

		[Theory]
		[AutoData]
		public void ShouldNotAddOnRollback(string[] itemsToAdd)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToAdd)
				{
					Sut.Add(item);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}

		[Theory]
		[AutoData]
		public void ShouldNotAddSameItemOnRollback(
			[Range(1, 100)] int numberOfSameItemInsertions,
			[Range(0, MinNumberOfInitialItems - 1)] int randomIndex)
		{
			var sameItem = Sut[randomIndex];

			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfSameItemInsertions; i++)
				{
					Sut.Add(sameItem);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}