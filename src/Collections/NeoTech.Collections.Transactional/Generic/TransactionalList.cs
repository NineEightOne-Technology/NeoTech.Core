using Microsoft;
using NeoTech.Core.Command;
using NeoTech.Core.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemTransactionScope = System.Transactions.TransactionScope;

namespace NeoTech.Collections.Transactional.Generic
{
	/// <summary>
	/// Acts as an implementation of an <see cref="IList{T}"/> that enables and adheres to the usage of <see cref="SystemTransactionScope"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The generic type parameter of the <see cref="TransactionalList{T}"/>.
	/// </typeparam>
	/// <remarks>
	/// This <see cref="TransactionalList{T}"/> works inside as well as outside of transactions.
	/// </remarks>
	public sealed class TransactionalList<T>
		: ICollection<T>
		, IEnumerable<T>
		, IEnumerable
		, IList<T>
		, IReadOnlyCollection<T>
		, IReadOnlyList<T>
		, ICollection
		, IList
	{
		private readonly UndoCommandManager _commandManager;
		private readonly SinglePhaseVolatileTransactionManager _transactionManager;

		private readonly List<T> _internalStorage = new List<T>();

		/// <summary>
		/// Creates a new instance of the <see cref="TransactionalList{T}"/> class.
		/// </summary>
		public TransactionalList()
		{
			_commandManager = new UndoCommandManager();

			_transactionManager = new SinglePhaseVolatileTransactionManager();
			_transactionManager.AddExecuteCallback(() => _commandManager.Reset());
			_transactionManager.AddRollbackCallback(() => _commandManager.UndoAll());
		}

		/// <inheritdoc />
		public T this[int index]
		{
			get => _internalStorage[index];
			set
			{
				if (_transactionManager.TrySubscribe())
				{
					var indexScoped = index;
					var currentItemScoped = _internalStorage[index];

					_commandManager.AddCommand(() => _internalStorage[indexScoped] = currentItemScoped);
				}

				_internalStorage[index] = value;
			}
		}

		/// <inheritdoc />
		object IList.this[int index]
		{
			get => this[index];
			set
			{
				Requires.Argument(value is T, nameof(value), "The value is not of the generic type T.");

				this[index] = (T)value;
			}
		}

		/// <inheritdoc />
		public int Count => _internalStorage.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <inheritdoc />
		public bool IsFixedSize => ((IList)_internalStorage).IsFixedSize;

		/// <inheritdoc />
		public bool IsSynchronized => ((ICollection)_internalStorage).IsSynchronized;

		/// <inheritdoc />
		public object SyncRoot => ((ICollection)_internalStorage).SyncRoot;

		/// <inheritdoc />
		public void Add(T item) => Add((object)item);

		/// <inheritdoc />
		public int Add(object value)
		{
			var insertionIndex = ((IList)_internalStorage).Add(value);

			if (_transactionManager.TrySubscribe())
				_commandManager.AddCommand(() => ((IList)_internalStorage).RemoveAt(insertionIndex));

			return insertionIndex;
		}

		/// <inheritdoc />
		public void Clear()
		{
			if (_transactionManager.TrySubscribe())
			{
				var currentItemsScoped = _internalStorage.ToArray();

				_commandManager.AddCommand(() =>
				{
					foreach (var item in currentItemsScoped)
						_internalStorage.Add(item);
				});
			}

			_internalStorage.Clear();
		}

		/// <inheritdoc />
		public bool Contains(T item) => _internalStorage.Contains(item);

		/// <inheritdoc />
		public bool Contains(object value)
		{
			Requires.Argument(value is T, nameof(value), "The value is not of the generic type T.");

			return Contains((T)value);
		}

		/// <inheritdoc />
		public void CopyTo(T[] array, int arrayIndex) => CopyTo((Array)array, arrayIndex);

		/// <inheritdoc />
		public void CopyTo(Array array, int index) => ((ICollection)_internalStorage).CopyTo(array, index);

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator() => _internalStorage.GetEnumerator();

		/// <inheritdoc />
		public int IndexOf(T item) => _internalStorage.IndexOf(item);

		/// <inheritdoc />
		public int IndexOf(object value)
		{
			Requires.Argument(value is T, nameof(value), "The value is not of the generic type T.");

			return IndexOf((T)value);
		}

		/// <inheritdoc />
		public void Insert(int index, T item)
		{
			if (_transactionManager.TrySubscribe())
			{
				var scopedIndex = index;

				_commandManager.AddCommand(() => _internalStorage.RemoveAt(index));
			}

			_internalStorage.Insert(index, item);
		}

		/// <inheritdoc />
		public void Insert(int index, object value)
		{
			Requires.Argument(value is T, nameof(value), "The value is not of the generic type T.");

			Insert(index, (T)value);
		}

		/// <inheritdoc />
		public bool Remove(T item)
		{
			var itemIndex = _internalStorage.IndexOf(item);

			if (itemIndex >= 0)
				RemoveAt(itemIndex);

			return itemIndex >= 0;
		}

		/// <inheritdoc />
		public void Remove(object value)
		{
			Requires.Argument(value is T, nameof(value), "The value is not of the generic type T.");

			Remove((T)value);
		}

		/// <inheritdoc />
		public void RemoveAt(int index)
		{
			var currentItemScoped = _internalStorage[index];

			_internalStorage.RemoveAt(index);

			if (_transactionManager.TrySubscribe())
			{
				var indexScoped = index;

				_commandManager.AddCommand(() => _internalStorage.Insert(indexScoped, currentItemScoped));
			}
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}