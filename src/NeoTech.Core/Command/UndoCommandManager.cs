using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft;

namespace NeoTech.Core.Command
{
	/// <summary>
	/// A command manager that uses an internal <see cref="Stack{T}"/> to store commands. This is most useful to revert actions in the correct order.
	/// </summary>
	public sealed class UndoCommandManager : ICommandManager
	{
		private readonly Stack<Action> _undoCommands;
		private readonly UndoCommandManagerOptions _options;

		public UndoCommandManager()
			: this(new UndoCommandManagerOptions()) { }

		public UndoCommandManager(UndoCommandManagerOptions options)
		{
			Requires.NotNull(options, nameof(options));

			_undoCommands = new Stack<Action>();
			_options = options;
		}

		public int Count => _undoCommands.Count;

		/// <summary>
		/// Adds an action to this command manager.
		/// </summary>
		/// <param name="command">
		/// The action to be added.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="command"/> is <c>null</c>.
		/// </exception>
		public void AddCommand(Action command)
		{
			Requires.NotNull(command, nameof(command));

			if (_undoCommands.Count == _options.CommandMemory)
				throw new InvalidOperationException($"This command manager has a memory of \"{_options.CommandMemory}\" items. The memory limit is reached.");

			_undoCommands.Push(command);
		}

		public void Undo(int numberOfSteps)
		{
			Requires.Argument(
				numberOfSteps <= _undoCommands.Count,
				nameof(numberOfSteps),
				"Cannot undo more steps than have been queued.");

			var startCount = _undoCommands.Count;

			while (_undoCommands.Count > startCount - numberOfSteps)
			{
				var undoAction = _undoCommands.Pop();

				undoAction();
			}

			_undoCommands.TrimExcess();
		}

		public void UndoAll()
		{
			Undo(_undoCommands.Count);
		}

		/// <summary>
		/// Executes all actions on the internal stack.
		/// </summary>
		/// <remarks>
		/// Each action will be popped from the internal <see cref="Stack{T}"/>. This means that this will only execute the stored actions once.
		/// </remarks>
		void ICommandManager.Execute()
		{
			UndoAll();
		}

		public IEnumerator<Action> GetEnumerator() => _undoCommands.GetEnumerator();

		/// <summary>
		/// Resets the internal <see cref="Stack{T}"/>.
		/// </summary>
		public void Reset()
		{
			_undoCommands.Clear();
			_undoCommands.TrimExcess();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}