using FluentAssertions;
using System.Transactions;
using Xunit;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public sealed class WhenClearing : TransactionalQueueTestBase<string>
	{
		[Fact]
		public void ShouldClearWithoutTransaction()
		{
			Sut.Clear();

			Sut
				.Should().BeEmpty();
		}

		[Fact]
		public void ShouldClearOnCommit()
		{
			using (var transaction = new TransactionScope())
			{
				Sut.Clear();

				transaction.Complete();
			}

			Sut
				.Should().BeEmpty();
		}

		[Fact]
		public void ShouldNotClearOnRollback()
		{
			using (var transaction = new TransactionScope())
			{
				Sut.Clear();
			}

			Sut
				.Should().BeEquivalentTo(InitialItems);
		}

		[Fact]
		public void ShouldClearWithPriorRollback()
		{
			using (var transaction = new TransactionScope())
			{
				Sut.Clear();
			}

			using (var transaction = new TransactionScope())
			{
				Sut.Clear();

				transaction.Complete();
			}

			Sut
				.Should().BeEmpty();
		}
	}
}