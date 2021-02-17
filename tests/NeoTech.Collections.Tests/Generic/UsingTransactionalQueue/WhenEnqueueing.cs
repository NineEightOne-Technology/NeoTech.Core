using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenEnqueueing : TransactionalQueueTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldEnqueueWithoutTransaction(string[] itemsToEnqueue)
		{
			foreach (var item in itemsToEnqueue)
			{
				Sut.Enqueue(item);
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToEnqueue.Length);

			Sut
				.Should().Contain(itemsToEnqueue);
		}

		[Theory]
		[AutoData]
		public void ShouldEnqueueOnCommit(string[] itemsToEnqueue)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToEnqueue)
				{
					Sut.Enqueue(item);
				}

				transactionScope.Complete();
			}

			Sut
				.Should().NotBeEquivalentTo(InitialItems)
				.And.HaveCount(InitialItems.Count + itemsToEnqueue.Length);

			Sut
				.Should().Contain(itemsToEnqueue);
		}

		[Theory]
		[AutoData]
		public void ShouldNotEnqueueOnRollback(string[] itemsToEnqueue)
		{
			using (var transactionScope = new TransactionScope())
			{
				foreach (var item in itemsToEnqueue)
				{
					Sut.Enqueue(item);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}

		[Theory]
		[AutoData]
		public void ShouldNotEnqueueSameItemOnRollback(
			[Range(1, 100)] int numberOfSameItemEnqueues,
			[Range(0, MinNumberOfInitialItems - 1)] int randomIndex)
		{
			var sameItem = Sut.ElementAt(randomIndex);

			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfSameItemEnqueues; i++)
				{
					Sut.Enqueue(sameItem);
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}