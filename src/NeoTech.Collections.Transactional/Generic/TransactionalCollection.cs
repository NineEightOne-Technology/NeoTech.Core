using NeoTech.Core.Transactions;
using System.Collections.ObjectModel;
using System.Linq;
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
		private readonly TransactionManager _transactionManager;
		private readonly StackCommandManager _commandManager;

		/// <summary>
		/// Creates a new instance of the <see cref="TransactionalCollection{T}"/> class.
		/// </summary>
		public TransactionalCollection()
		{
			_commandManager = new StackCommandManager();

			_transactionManager = new TransactionManager
			{
				OnCommittingAction = () => _commandManager.Reset(),
				OnRollingBackAction = () => _commandManager.Execute()
			};
		}

		/// <inheritdoc />
		protected override void ClearItems()
		{
			if (_transactionManager.HookToCurrentTransaction())
			{
				var scopedItems = Items.ToArray();

				_commandManager.AddAction(() =>
				{
					foreach (var item in scopedItems)
						Add(item);
				});
			}

			base.ClearItems();
		}

		/// <inheritdoc />
		protected override void InsertItem(int index, T item)
		{
			if (_transactionManager.HookToCurrentTransaction())
			{
				var scopedIndex = index;

				_commandManager.AddAction(() => base.RemoveItem(scopedIndex));
			}

			base.InsertItem(index, item);
		}

		/// <inheritdoc />
		protected override void RemoveItem(int index)
		{
			if (_transactionManager.HookToCurrentTransaction())
			{
				int scopedIndex = index;
				T itemScoped = this[scopedIndex];

				_commandManager.AddAction(() => base.InsertItem(scopedIndex, itemScoped));
			}

			base.RemoveItem(index);
		}

		/// <inheritdoc />
		protected override void SetItem(int index, T item)
		{
			if (_transactionManager.HookToCurrentTransaction())
			{
				int indexScoped = index;
				T itemScoped = this[indexScoped];

				_commandManager.AddAction(() => base.SetItem(indexScoped, itemScoped));
			}

			base.SetItem(index, item);
		}
	}
}