using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenInserting : TransactionalListTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldInsertWithoutTransaction(string[] itemsToInsert)
		{
			foreach (var item in itemsToInsert)
			{
				var randomInsertionIndex = RandomNumberGenerator.Next(0, Sut.Count - 1);

				Sut.Insert(randomInsertionIndex, (object)item);

				Sut[randomInsertionIndex]
					.Should().Be(item);
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToInsert.Length);
		}

		[Theory]
		[AutoData]
		public void ShouldInsertOnCommit(string[] itemsToInsert)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToInsert)
				{
					var randomInsertionIndex = RandomNumberGenerator.Next(0, Sut.Count - 1);

					Sut.Insert(randomInsertionIndex, (object) item);

					Sut[randomInsertionIndex]
						.Should().Be(item);
				}

				transactionScope.Complete();
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToInsert.Length);
		}

		[Theory]
		[AutoData]
		public void ShouldNotInsertOnRollback(string[] itemsToInsert)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToInsert)
				{
					var randomInsertionIndex = RandomNumberGenerator.Next(0, Sut.Count - 1);

					Sut.Insert(randomInsertionIndex, (object)item);

					Sut[randomInsertionIndex]
						.Should().Be(item);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}

		[Theory]
		[AutoData]
		public void ShouldNotInsertExistingItemOnRollback([Range(1, MinNumberOfInitialItems)] int numberOfExistingItemInsertions)
		{
			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfExistingItemInsertions; i++)
				{
					var randomPickIndex = RandomNumberGenerator.Next(0, InitialItems.Count - 1);
					var randomInsertIndex = RandomNumberGenerator.Next(0, InitialItems.Count - 1);

					var sameItem = InitialItems.ElementAt(randomPickIndex);

					Sut.Insert(randomInsertIndex, (object) sameItem);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}