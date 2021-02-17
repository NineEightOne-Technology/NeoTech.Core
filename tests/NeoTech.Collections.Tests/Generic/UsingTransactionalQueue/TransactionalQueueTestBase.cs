using AutoFixture;
using NeoTech.Collections.Transactional.Generic;
using System;
using System.Collections.Generic;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalQueue
{
	public abstract class TransactionalQueueTestBase<T>
	{
		protected static readonly Random RandomNumberGenerator = new Random();

		protected const int MinNumberOfInitialItems = 100;
		protected const int MaxNumberOfInitialItems = 1000;

		public TransactionalQueueTestBase()
		{
			Sut = new TransactionalQueue<T>();

			var numberOfItemsToAdd = RandomNumberGenerator.Next(MinNumberOfInitialItems, MaxNumberOfInitialItems);

			for (int i = 0; i < numberOfItemsToAdd; i++)
			{
				var item = Fixture.Create<T>();

				Sut.Enqueue(item);
			}

			InitialItems = Sut.ToArray();
		}

		protected IFixture Fixture { get; }
			= new Fixture();

		protected TransactionalQueue<T> Sut { get; }

		protected IReadOnlyCollection<T> InitialItems { get; }
	}
}