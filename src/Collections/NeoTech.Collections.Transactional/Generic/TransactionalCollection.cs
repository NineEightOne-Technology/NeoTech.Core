using System.Collections.ObjectModel;
using System.Linq;
using NeoTech.Core.Command;
using NeoTech.Core.Transactions;
using SystemTransactionScope = System.Transactions.TransactionScope;

namespace NeoTech.Collections.Transactional.Generic
{
	/// <summary>
	/// A collection that enables and adheres to the usage of <see cref="SystemTransactionScope"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The generic type parameter of the collection.
	/// </typeparam>
	public class TransactionalCollection<T> : Collection<T>
	{
		private readonly UndoCommandManager _commandManager;

		private readonly SinglePhaseVolatileTransactionManager _transactionManager;

		/// <summary>
		/// Creates a new instance of the <see cref="TransactionalCollection{T}"/> class.
		/// </summary>
		public TransactionalCollection()
		{
			_commandManager = new UndoCommandManager();

			_transactionManager = new SinglePhaseVolatileTransactionManager();
			_transactionManager.AddExecuteCallback(() => _commandManager.Reset());
			_transactionManager.AddRollbackCallback(() => _commandManager.UndoAll());
		}
		/// <inheritdoc />
		protected override void ClearItems()
		{
			if (_transactionManager.TrySubscribe())
			{
				var scopedItems = Items.ToArray();

				_commandManager.AddCommand(() =>
				{
					for (int i = 0; i < scopedItems.Length; i++)
					{
						var currentItem = scopedItems[i];

						base.InsertItem(i, currentItem);
					}
				});
			}

			base.ClearItems();
		}

		/// <inheritdoc />
		protected override void InsertItem(int index, T item)
		{
			if (_transactionManager.TrySubscribe())
			{
				var scopedIndex = index;

				_commandManager.AddCommand(() => base.RemoveItem(scopedIndex));
			}

			base.InsertItem(index, item);
		}

		/// <inheritdoc />
		protected override void RemoveItem(int index)
		{

			if (_transactionManager.TrySubscribe())
			{
				int scopedIndex = index;
				T itemScoped = this[scopedIndex];

				_commandManager.AddCommand(() => base.InsertItem(scopedIndex, itemScoped));
			}

			base.RemoveItem(index);
		}

		/// <inheritdoc />
		protected override void SetItem(int index, T item)
		{
			if (_transactionManager.TrySubscribe())
			{
				int indexScoped = index;
				T itemScoped = this[indexScoped];

				_commandManager.AddCommand(() => base.SetItem(indexScoped, itemScoped));
			}

			base.SetItem(index, item);
		}
	}
}