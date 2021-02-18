using System.Transactions;
using FluentAssertions;
using NeoTech.Core.Transactions;
using Xunit;

namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
{
	public sealed class WhenSubscribing
	{
		private readonly ITransactionManager _sut;

		public WhenSubscribing()
		{
			_sut = new TwoPhaseVolatileTransactionManager();
		}

		[Fact]
		public void ShouldSubscribeOnExistingTransaction()
		{
			using (new TransactionScope())
			{
				_sut.TrySubscribe();

				_sut.IsSubscribed
					.Should().BeTrue();
			}
		}

		[Fact]
		public void ShouldUnsubscribeOnCommit()
		{
			using (var transactionScope = new TransactionScope())
			{
				_sut.TrySubscribe();

				transactionScope.Complete();
			}

			_sut.IsSubscribed
				.Should().BeFalse();
		}

		[Fact]
		public void ShouldUnsubscribeOnRollback()
		{
			using (new TransactionScope())
			{
				_sut.TrySubscribe();
			}

			_sut.IsSubscribed
				.Should().BeFalse();
		}

		[Fact]
		public void ShouldNotSubscribeOnNonExistingTransaction()
		{
			_sut.TrySubscribe();

			_sut.IsSubscribed
				.Should().BeFalse();
		}
	}
}