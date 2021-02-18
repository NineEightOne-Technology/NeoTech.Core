using System;
using FluentAssertions;
using NeoTech.Core.Command;
using NeoTech.Transaction.Tests.UsingStackCommandManager;
using Xunit;

namespace NeoTech.Core.Tests.UsingUndoCommandManager
{
	public sealed class WhenInstantiating : UndoCommandManagerTestBase
	{
		[Fact]
		public void ShouldThrowOnOptionsNull()
		{
			Action ctorAction = () =>
			{
				new UndoCommandManager(null);
			};

			ctorAction
				.Invoking(x => x())
				.Should().ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void ShouldNotThrowOnDefaultConstructor()
		{
			Action ctorAction = () =>
			{
				new UndoCommandManager();
			};

			ctorAction
				.Invoking(x => x())
				.Should().NotThrow();
		}
	}
}