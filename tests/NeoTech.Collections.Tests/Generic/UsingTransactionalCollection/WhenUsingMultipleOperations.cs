using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public sealed class WhenUsingMultipleOperations : TransactionalCollectionTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldCorrectlyRollback(
			string[] itemsToAdd,
			[Range(10, MinNumberOfInitialItems)] int numberOfOperations)
		{
			using (var transactionScope = new TransactionScope())
			{
				for (int i = 0; i < numberOfOperations; i++)
				{
					var indexToRemove = RandomNumberGenerator.Next(0, Sut.Count - 1);
					Sut.RemoveAt(indexToRemove);

					foreach (var item in itemsToAdd)
						Sut.Add(item);

					var indexOfExistingItem = RandomNumberGenerator.Next(0, Sut.Count);
					var itemToRemove = Sut[indexOfExistingItem];
					bool removalResult = Sut.Remove(itemToRemove);

					removalResult
						.Should().BeTrue();

					Sut.Clear();

					foreach (var item in itemsToAdd.Reverse())
						Sut.Add(item);

					Sut.Remove(Fixture.Create<string>());
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}