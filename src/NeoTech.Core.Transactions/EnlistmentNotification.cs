using Microsoft;
using System;
using System.Transactions;
using SystemTransactionManager = System.Transactions.TransactionManager;

namespace NeoTech.Core.Transactions
{
	/// <summary>
	/// Acts as an <see cref="IEnlistmentNotification"/> implementation offering pre and post events for each
	/// of the possible transaction notifications.
	/// </summary>
	public sealed class EnlistmentNotification : IEnlistmentNotification
	{
		/// <summary>
		/// Raised before sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnCommitting;

		/// <summary>
		/// Raised after sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnCommit;

		/// <summary>
		/// Raised before sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnRollingBack;

		/// <summary>
		/// Raised after sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnRollback;

		/// <summary>
		/// Raised before sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnPreparing;

		/// <summary>
		/// Raised after sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnPrepared;

		/// <summary>
		/// Raised before sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnDoubting;

		/// <summary>
		/// Raised after sending the response to the <see cref="SystemTransactionManager"/>.
		/// </summary>
		public event EventHandler OnDoubted;

		/// <summary>
		///	Notifies an enlisted object that a transaction is being committed. Raises the <see cref="OnCommitting"/>
		/// event prior and the <see cref="OnCommit"/> event after notifying the <see cref="SystemTransactionManager"/>.
		/// </summary>
		/// <param name="enlistment">
		/// An <see cref="Enlistment"/> object used to send a response to the <see cref="SystemTransactionManager"/>.
		/// </param>
		public void Commit(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			OnCommitting?.Invoke(this, EventArgs.Empty);

			enlistment.Done();

			OnCommit?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Notifies an enlisted object that the status of a transaction is in doubt. Raises the <see cref="OnDoubting"/>
		/// event prior and the <see cref="OnDoubted"/> event after notifying the <see cref="SystemTransactionManager"/>.
		/// </summary>
		/// <param name="enlistment">
		/// An <see cref="Enlistment"/> object used to send a response to the <see cref="SystemTransactionManager"/>.
		/// </param>
		public void InDoubt(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			OnDoubting?.Invoke(this, EventArgs.Empty);

			Rollback(enlistment);

			OnDoubted?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///  Notifies an enlisted object that a transaction is being prepared for commitment. Raises the <see cref="OnPreparing"/>
		/// event prior and the <see cref="OnPrepared"/> event after notifying the <see cref="SystemTransactionManager"/>.
		/// </summary>
		/// <param name="preparingEnlistment">
		/// A <see cref="PreparingEnlistment"/> object used to send a response to the <see cref="SystemTransactionManager"/>.
		/// </param>
		public void Prepare(PreparingEnlistment preparingEnlistment)
		{
			Requires.NotNull(preparingEnlistment, nameof(preparingEnlistment));

			OnPreparing?.Invoke(this, EventArgs.Empty);

			preparingEnlistment.Prepared();

			OnPrepared?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Notifies an enlisted object that a transaction is being rolled back (aborted). Raises the <see cref="OnRollback"/>
		/// event prior and the <see cref="OnRollingBack"/> event after notifying the <see cref="SystemTransactionManager"/>.
		/// </summary>
		/// <param name="enlistment">
		/// An <see cref="Enlistment"/> object used to send a response to the <see cref="SystemTransactionManager"/>.
		/// </param>
		public void Rollback(Enlistment enlistment)
		{
			Requires.NotNull(enlistment, nameof(enlistment));

			OnRollingBack?.Invoke(this, EventArgs.Empty);

			enlistment.Done();

			OnRollback?.Invoke(this, EventArgs.Empty);
		}
	}
}