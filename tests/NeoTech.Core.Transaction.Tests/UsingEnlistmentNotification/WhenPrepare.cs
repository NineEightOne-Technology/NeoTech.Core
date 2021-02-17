//using FluentAssertions;
//using System;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingEnlistmentNotification
//{
//	public sealed class WhenPrepare : EnlistmentNotificationTestBase
//	{
//		[Fact]
//		public void ShouldThrowOnEnlistmentNull()
//		{
//			Sut
//				.Invoking(x => x.Prepare(null))
//				.Should().ThrowExactly<ArgumentNullException>();
//		}
//	}
//}