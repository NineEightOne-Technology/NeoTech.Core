using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeoTech.Collections.Tests.Generic
{
	public abstract class CollectionTestBase<TSutType, TItemType>
		where TSutType: ICollection<TItemType>
	{
		protected static readonly Random RandomNumberGenerator = new Random();

		protected const int MinNumberOfInitialItems = 100;
		protected const int MaxNumberOfInitialItems = 1000;

		public CollectionTestBase()
		{
			Fixture.AddManyTo(
				Sut,
				RandomNumberGenerator.Next(MinNumberOfInitialItems, MaxNumberOfInitialItems));

			InitialItems = Sut.ToArray();
		}

		protected IFixture Fixture { get; }
			= new Fixture();

		protected abstract TSutType Sut { get; }

		protected IReadOnlyCollection<TItemType> InitialItems { get; }
	}
}
