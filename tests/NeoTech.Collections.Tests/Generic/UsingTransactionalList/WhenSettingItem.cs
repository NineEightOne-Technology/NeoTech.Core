using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public sealed class WhenSettingItem : TransactionalListTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldSetWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<object, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			foreach (var randomIndex in randomIndexes)
			{
				object randomItem = Fixture.Create<string>();

				setTrace.Add(randomItem, randomIndex);

				SutWithObjects[randomIndex] = randomItem;
			}

			using (new AssertionScope())
			{
				foreach (var item in setTrace)
				{
					SutWithObjects[item.Value]
						.Should().Be(item.Key);
				}
			}
		}

		[Theory]
		[AutoData]
		public void ShouldSetOnCommit([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<object, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			using (var transaction = new TransactionScope())
			{
				foreach (var randomIndex in randomIndexes)
				{
					object randomItem = Fixture.Create<string>();

					setTrace.Add(randomItem, randomIndex);

					SutWithObjects[randomIndex] = randomItem;
				}

				transaction.Complete();
			}

			using (new AssertionScope())
			{
				foreach (var item in setTrace)
				{
					SutWithObjects[item.Value]
						.Should().Be(item.Key);
				}
			}
		}

		[Theory]
		[AutoData]
		public void ShouldNotSetOnRollback([Range(1, MinNumberOfInitialItems)] int numberOfSetOperations)
		{
			var setTrace = new Dictionary<object, int>();

			var randomIndexes = GetRandomIndexes(numberOfSetOperations);

			using (var transaction = new TransactionScope())
			{
				foreach (var randomIndex in randomIndexes)
				{
					object randomItem = Fixture.Create<string>();

					setTrace.Add(randomItem, randomIndex);

					SutWithObjects[randomIndex] = randomItem;
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}

		[Theory]
		[AutoData]
		public void ShouldThrowIfItemIsNotOfGenericType([Range(1, MinNumberOfInitialItems)] int randomIndex)
		{
			Sut
				.Invoking(x => ((IList)x)[randomIndex] = new object())
				.Should().ThrowExactly<ArgumentException>();
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
