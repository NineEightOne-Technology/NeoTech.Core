using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public sealed class WhenSettingItem : TransactionalCollectionTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldSetWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<string, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			foreach (var randomIndex in randomIndexes)
			{
				var randomItem = Fixture.Create<string>();

				setTrace.Add(randomItem, randomIndex);

				Sut[randomIndex] = randomItem;
			}

			using (new AssertionScope())
			{
				foreach (var item in setTrace)
				{
					Sut[item.Value]
						.Should().Be(item.Key);
				}
			}
		}

		[Theory]
		[AutoData]
		public void ShouldSetOnCommit([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<string, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			using (var transaction = new TransactionScope())
			{
				foreach (var randomIndex in randomIndexes)
				{
					var randomItem = Fixture.Create<string>();

					setTrace.Add(randomItem, randomIndex);

					Sut[randomIndex] = randomItem;
				}

				transaction.Complete();
			}

			using (new AssertionScope())
			{
				foreach (var item in setTrace)
				{
					Sut[item.Value]
						.Should().Be(item.Key);
				}
			}
		}

		[Theory]
		[AutoData]
		public void ShouldNotSetOnRollback([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<string, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			using (var transaction = new TransactionScope())
			{
				foreach (var randomIndex in randomIndexes)
				{
					var randomItem = Fixture.Create<string>();

					setTrace.Add(randomItem, randomIndex);

					Sut[randomIndex] = randomItem;
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}

		private ISet<int> GetRandomIndexes(int numberOfIndexesToPick)
		{
			var result = new HashSet<int>();

			for (int i = 0; i < numberOfIndexesToPick; i++)
			{
				var randomIndex = RandomNumberGenerator.Next(0, Sut.Count);

				result.Add(randomIndex);
			}

			return result;
		}
	}
}
