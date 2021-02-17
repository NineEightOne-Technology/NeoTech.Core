//using FluentAssertions;
//using Moq;
//using System;
//using Xunit;

//namespace NeoTech.Transaction.Tests.UsingStackCommandManager
//{
//	public sealed class WhenAddingAction : StackCommandManagerTestBase
//	{
//		[Fact]
//		public void ShouldThrowOnActionNull()
//		{
//			Sut
//				.Invoking(x => x.AddAction(null))
//				.Should().ThrowExactly<ArgumentNullException>();
//		}

//		[Fact]
//		public void ShouldAddActionToExecutionStack()
//		{
//			var action = new Mock<Action>();

//			Sut.AddAction(action.Object);
//			Sut.Execute();

//			action.Verify(a => a(), Times.Once);
//		}
//	}
//}