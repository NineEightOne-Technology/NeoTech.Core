using NeoTech.Collections.Tests.Generic;
using NeoTech.Collections.Transactional.Generic;

namespace NeoTech.Collections.Transactional.Tests.Generic.UsingTransactionalCollection
{
	public abstract class TransactionalCollectionTestBase<T> : CollectionTestBase<TransactionalCollection<T>, T>
	{
		protected override TransactionalCollection<T> Sut { get; } = new TransactionalCollection<T>();
	}
}