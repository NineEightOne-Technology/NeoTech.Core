using NeoTech.Collections.Transactional.Generic;
using System.Collections;

namespace NeoTech.Collections.Tests.Generic.UsingTransactionalList
{
	public abstract class TransactionalListTestBase<T> : CollectionTestBase<TransactionalList<T>, T>
	{
		protected override TransactionalList<T> Sut { get; }
			= new TransactionalList<T>();

		protected IList SutWithObjects => Sut;
	}
}