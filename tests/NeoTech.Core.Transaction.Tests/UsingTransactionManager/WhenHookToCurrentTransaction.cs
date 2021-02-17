//using FluentAssertions;
//using System.Transactions;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotificationManager
//{
//	public sealed class WhenHookToCurrentTransaction : TransactionManagerTestBase
//	{
//		[Fact]
//		public void ShouldBeTrueWithTransaction()
//		{
//			bool result = false;

//			using (var transationScope = new TransactionScope())
//			{
//				result = Sut.HookToCurrentTransaction();
//			}

//			result
//				.Should().BeTrue();
//		}

//		[Fact]
//		public void ShouldBeFalseWithoutTransaction()
//		{
//			var result = Sut.HookToCurrentTransaction();

//			result
//				.Should().BeFalse();
//		}
//	}
//}