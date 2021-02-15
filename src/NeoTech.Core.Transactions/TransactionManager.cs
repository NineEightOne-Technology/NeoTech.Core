using System;
using System.Transactions;

namespace NeoTech.Transactions
{
	/// <summary>
	/// Acts as a manager that can subscribe itself to an occuring transaction. This manager provides callbacks for the commit and rollback events of a transaction.
	/// </summary>
	public sealed class TransactionManager
	{
		private EnlistmentNotification _enlistmentNotification;

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked prior to acknowledging the successful rollback to
		/// the <see cref="Transaction"/> instance. This is where actual code to rollback changes should be executed.
		/// </summary>
		public Action OnRollingBackAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked after the successful rollback has been communicated to
		/// the <see cref="Transaction"/> instance. This is where any clean up code can be executed.
		/// </summary>
		/// <remarks>
		/// No more changes to the actual data should occur here, as the rollback has already been communicated to the <see cref="Transaction"/>.
		/// </remarks>
		public Action OnRollbackAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked prior to acknowledging the successful commit to the <see cref="Transaction"/> instance.
		/// This is where actual code to commit changes should be executed.
		/// </summary>
		public Action OnCommittingAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked after the successful commit has been communicated to
		/// the <see cref="Transaction"/> instance. This is where any clean up code can be executed.
		/// </summary>
		/// <remarks>
		/// No more changes to the actual data should occur here, as the commit has already been communicated to the <see cref="Transaction"/>.
		/// </remarks>
		public Action OnCommitAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked prior to acknowledging the successful preparing message
		/// to the <see cref="Transaction"/> instance. This is where actual code to prepare should be executed.
		/// </summary>
		public Action OnPreparingAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked after the successful preparation has been communicated to
		/// the <see cref="Transaction"/> instance.
		/// </summary>
		public Action OnPreparedAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked prior to acknowledging the successful consumption of
		/// the doubting notification to the <see cref="Transaction"/> instance.
		/// </summary>
		public Action OnDoubtingAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked after acknowledging the successful consumption of
		/// the doubting notification to the <see cref="Transaction"/> instance.
		/// </summary>
		public Action OnDoubtedAction { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="Action"/> that will be invoked upon completion of the transaction.
		/// </summary>
		public Action<Transaction> OnTransactionCompletedAction { get; set; }

		/// <summary>
		/// This method will
		/// </summary>
		/// <returns></returns>
		public bool HookToCurrentTransaction()
		{
			bool isCurrentlyInTransaction = false;

			var currentTransaction = Transaction.Current;

			if (currentTransaction != null)
			{
				if (_enlistmentNotification == null)
				{
					_enlistmentNotification = new EnlistmentNotification();

					_enlistmentNotification.OnCommitting += OnCommitting;
					_enlistmentNotification.OnCommit += OnCommit;

					_enlistmentNotification.OnRollingBack += OnRollingBack;
					_enlistmentNotification.OnRollback += OnRollback;

					_enlistmentNotification.OnDoubting += OnDoubting;
					_enlistmentNotification.OnDoubted += OnDoubted;

					_enlistmentNotification.OnPreparing += OnPreparing;
					_enlistmentNotification.OnPrepared += OnPrepared;


					currentTransaction.TransactionCompleted += OnTransactionCompleted;

					currentTransaction.EnlistVolatile(_enlistmentNotification, EnlistmentOptions.None);
				}

				isCurrentlyInTransaction = true;
			}

			return isCurrentlyInTransaction;
		}


		private void OnTransactionCompleted(object sender, TransactionEventArgs e)
		{
			OnTransactionCompletedAction?.Invoke(e.Transaction);

			var transaction = sender as Transaction;

			if (transaction != null)
			{
				transaction.TransactionCompleted -= OnTransactionCompleted;
			}
		}

		private void OnCommitting(object sender, EventArgs e)
		{
			OnCommittingAction?.Invoke();
		}

		private void OnCommit(object sender, EventArgs e)
		{
			OnCommitAction?.Invoke();
		}

		private void OnRollingBack(object sender, EventArgs e)
		{
			OnRollingBackAction?.Invoke();
		}

		private void OnRollback(object sender, EventArgs e)
		{
			OnRollbackAction?.Invoke();
		}

		private void OnPrepared(object sender, EventArgs e)
		{
			OnPreparedAction?.Invoke();

			if (_enlistmentNotification != null)
			{
				_enlistmentNotification.OnCommit -= OnCommit;
				_enlistmentNotification.OnCommitting -= OnCommitting;

				_enlistmentNotification.OnRollback -= OnRollback;
				_enlistmentNotification.OnRollingBack -= OnRollingBack;

				_enlistmentNotification.OnDoubted -= OnDoubted;
				_enlistmentNotification.OnDoubting -= OnDoubting;

				_enlistmentNotification.OnRollingBack -= OnPrepared;
				_enlistmentNotification.OnPreparing -= OnPreparing;

				_enlistmentNotification = null;
			}
		}

		private void OnPreparing(object sender, EventArgs e)
		{
			OnPreparingAction?.Invoke();
		}

		private void OnDoubted(object sender, EventArgs e)
		{
			OnDoubtedAction?.Invoke();
		}

		private void OnDoubting(object sender, EventArgs e)
		{
			OnDoubtingAction?.Invoke();
		}
	}
}