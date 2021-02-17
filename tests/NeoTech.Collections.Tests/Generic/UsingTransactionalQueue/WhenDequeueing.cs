using AutoFixture.Xunit2;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenDequeueing : TransactionalQueueTestBase<string>
	{
		[Theory]
		[AutoData]
		public void ShouldDequeueWithoutTransaction([Range(1, MinNumberOfInitialItems)] int numberOfDequeues)
		{
			for (int i = 0; i < numberOfDequeues; i++)
			{
				Sut.Dequeue();
			}

			Sut
				.Should().HaveCount(InitialItems.Count - numberOfDequeues);
		}

		[Theory]
		[AutoData]
		public void ShouldDequeueOnCommit([Range(1, MinNumberOfInitialItems)] int numberOfDequeues)
		{
			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfDequeues; i++)
				{
					Sut.Dequeue();
				}

				transaction.Complete();
			}

			Sut
				.Should().HaveCount(InitialItems.Count - numberOfDequeues);
		}

		[Theory]
		[AutoData]
		public void ShouldNotDequeueOnRollback([Range(1, MinNumberOfInitialItems)] int numberOfDequeues)
		{
			using (var transaction = new TransactionScope())
			{
				for (int i = 0; i < numberOfDequeues; i++)
				{
					Sut.Dequeue();
				}
			}

			Sut
				.Should().BeEquivalentTo(InitialItems, o => o.WithStrictOrdering());
		}
	}
}