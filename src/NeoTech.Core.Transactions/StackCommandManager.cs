using System;
using System.Collections.Generic;
using Microsoft;

namespace NeoTech.Core.Transactions
{
	/// <summary>
	/// A command manager that uses an internal <see cref="Stack{T}"/> to store commands. This is most useful to revert actions in the correct order.
	/// </summary>
	public sealed class StackCommandManager
	{
		private readonly Stack<Action> _undoActions = new Stack<Action>();

		/// <summary>
		/// Adds an action to this command manager.
		/// </summary>
		/// <param name="action">
		/// The action to be added.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="action"/> is <c>null</c>.
		/// </exception>
		public void AddAction(Action action)
		{
			Requires.NotNull(action, nameof(action));

			_undoActions.Push(action);
		}

		/// <summary>
		/// Executes all actions on the internal stack.
		/// </summary>
		/// <remarks>
		/// Each action will be popped from the internal <see cref="Stack{T}"/>. This means that this will only execute the stored actions once.
		/// </remarks>
		public void Execute()
		{
			while (_undoActions.Count > 0)
			{
				var undoAction = _undoActions.Pop();

				undoAction();
			}
		}

		/// <summary>
		/// Resets the internal <see cref="Stack{T}"/>.
		/// </summary>
		public void Reset()
		{
			_undoActions.Clear();
		}
	}
}