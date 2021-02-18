using System;
using System.Transactions;

namespace NeoTech.Core.Transactions
{
	/// <summary>
	/// Indicates that an implementing entity can manage and react to <see cref="Transaction"/> notifications.
	/// </summary>
	public interface ITransactionManager
	{
		/// <summary>
		/// Adds an <see cref="Action"/> to the completion callback list.
		/// </summary>
		/// <param name="callback">
		/// An action to be invoked upon the signal of completion.
		/// </param>
		void AddCompletionCallback(Action callback);

		/// <summary>
		/// Adds an <see cref="Action"/> to the rollback callback list.
		/// </summary>
		/// <param name="callback">
		/// An action to be invoked upon the signal of rollback.
		void AddRollbackCallback(Action callback);

		/// <summary>
		/// Adds an <see cref="Action"/> to the executing callback list.
		/// </summary>
		/// <param name="callback">
		/// An action to be invoked upon the signal of execution.
		void AddExecuteCallback(Action callback);

		/// <summary>
		/// Tries to subscribe to a potentially active transaction.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this <see cref="ITransactionManager"/> successfully subscribed to an
		/// active <see cref="Transaction"/>, else <c>false</c>.
		/// </returns>
		bool TrySubscribe();
	}
}