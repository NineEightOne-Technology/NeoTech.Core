﻿using Microsoft;
using NeoTech.Core.Command;
using NeoTech.Core.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemTransactionScope = System.Transactions.TransactionScope;

namespace NeoTech.Collections.Transactional.Generic
{
	/// <summary>
	/// Acts as an implementation of a <see cref="Queue{T}"/> that enables and adheres to the usage of <see cref="SystemTransactionScope"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The generic type parameter of the <see cref="TransactionalQueue{T}"/>.
	/// </typeparam>
	/// <remarks>
	/// This <see cref="TransactionalQueue{T}"/> works inside as well as outside of transactions.
	/// </remarks>
	public sealed class TransactionalQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
	{
		private readonly UndoCommandManager _commandManager;

		private readonly SinglePhaseVolatileTransactionManager _transactionManager;

		private readonly Queue<T> _internalStorage = new Queue<T>();

		/// <summary>
		/// Creates a new instance of the <see cref="TransactionalQueue{T}"/> class.
		/// </summary>
		public TransactionalQueue()
		{
			_commandManager = new UndoCommandManager();

			_transactionManager = new SinglePhaseVolatileTransactionManager();
			_transactionManager.AddCompletionCallback(() => _commandManager.Reset());
			_transactionManager.AddRollbackCallback(() => _commandManager.UndoAll());
		}

		/// <inheritdoc />
		public int Count => _internalStorage.Count;

		/// <inheritdoc />
		public bool IsSynchronized => ((ICollection)_internalStorage).IsSynchronized;

		/// <inheritdoc />
		public object SyncRoot => ((ICollection)_internalStorage).SyncRoot;

		/// <inheritdoc />
		public void Clear()
		{
			if (_transactionManager.TrySubscribe())
			{
				var backup = new T[_internalStorage.Count];
				_internalStorage.CopyTo(backup, 0);

				_commandManager.AddCommand(
					() =>
					{
						foreach (var item in backup)
							_internalStorage.Enqueue(item);
					});
			}

			_internalStorage.Clear();
		}

		/// <inheritdoc />
		public bool Contains(T item) => _internalStorage.Contains(item);

		/// <inheritdoc />
		public void CopyTo(T[] array, int arrayIndex) => _internalStorage.CopyTo(array, arrayIndex);

		/// <inheritdoc />
		public T Dequeue()
		{
			T item = _internalStorage.Dequeue();

			if (_transactionManager.TrySubscribe())
			{
				_commandManager.AddCommand(
					() =>
					{
						T[] backup = new T[_internalStorage.Count + 1];
						backup[0] = item;
						CopyTo(backup, 1);

						_internalStorage.Clear();

						foreach (var backupItem in backup)
							_internalStorage.Enqueue(backupItem);
					});
			}

			return item;
		}

		/// <inheritdoc />
		public void Enqueue(T item)
		{
			if (_transactionManager.TrySubscribe())
			{
				var backup = new T[_internalStorage.Count];
				CopyTo(backup, 0);

				_commandManager.AddCommand(
					() =>
					{
						_internalStorage.Clear();

						foreach (var backupItem in backup)
							_internalStorage.Enqueue(backupItem);
					});
			}

			_internalStorage.Enqueue(item);
		}

		/// <inheritdoc />
		public T Peek() => _internalStorage.Peek();

		/// <inheritdoc />
		public T[] ToArray() => _internalStorage.ToArray();

		/// <inheritdoc />
		public void TrimExcess() => _internalStorage.TrimExcess();

		/// <inheritdoc />
		public void CopyTo(Array array, int index)
		{
			Requires.Argument(array is T[], nameof(array), "Array is not of generic type T.");

			CopyTo((T[])array, index);
		}

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator() => _internalStorage.GetEnumerator();

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}