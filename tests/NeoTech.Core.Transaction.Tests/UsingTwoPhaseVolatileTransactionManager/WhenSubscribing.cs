using System.ComponentModel.DataAnnotations;
using System.Transactions;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using NeoTech.Core.Transaction.Tests.UsingTwoPhasePhaseEnlistmentNotification;
using Xunit;

namespace NeoTech.Core.Transaction.Tests.UsingTwoPhaseVolatileTransactionManager
{
	public sealed class WhenSubscribing : TwoPhaseVolatileTransactionManagerTestBase
	{
		[Fact]
		public void ShouldSubscribeOnExistingTransaction()
		{
			using (new TransactionScope())
			{
				bool result = Sut.TrySubscribe();

				result
					.Should().BeTrue();
			}
		}

		[Fact]
		public void ShouldNotSubscribeOnNonExistingTransaction()
		{
			bool result = Sut.TrySubscribe();

			result
				.Should().BeFalse();
		}
	}
}