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

		/// <summary>
		/// Creates a new instance of the <see cref="UndoCommandManager"/> class.
		/// </summary>
		/// <seealso cref="UndoCommandManager(UndoCommandManagerOptions)"/>
		/// <remarks>
		/// Dipatches instantiation to <see cref="UndoCommandManager(UndoCommandManagerOptions)"/>.
		/// </remarks>
		public UndoCommandManager()
			: this(new UndoCommandManagerOptions()) { }

		/// <summary>
		/// Creates a new instance of the <see cref="UndoCommandManager"/> class.
		/// </summary>
		/// <param name="options">
		/// options to influence the bahavior of this command manager.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="options"/> is <c>null</c>.
		/// </exception>
		public UndoCommandManager(UndoCommandManagerOptions options)
		{
			Requires.NotNull(options, nameof(options));

			_undoCommands = new Stack<Action>();
			_options = options;
		}

		/// <summary>
		/// Gets the number of <see cref="Action"/>s that have been added.
		/// </summary>
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

		/// <summary>
		/// Pops <paramref name="numberOfSteps"/> command actions from the internal <see cref="Stack{T}"/> and
		/// executes the actions.
		/// </summary>
		/// <param name="numberOfSteps">
		/// The number of actions to pop and execute.
		/// </param>
		/// <exception cref="ArgumentException">
		/// If <paramref name="numberOfSteps"/> is greater than <see cref="Count"/>.
		/// </exception>
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

		/// <summary>
		/// Pops all commands from the internal stack and executes them.
		/// </summary>
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

		/// <summary>
		/// Gets an enumerator over all added commands.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator{T}"/> over all added commands.
		/// </returns>
		public IEnumerator<Action> GetEnumerator() => _undoCommands.GetEnumerator();

		/// <summary>
		/// Resets the internal <see cref="Stack{T}"/>.
		/// </summary>
		public void Reset()
		{
			_undoCommands.Clear();
			_undoCommands.TrimExcess();
		}

		/// <summary>
		/// Gets an enumerator over all added commands.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator"/> over all added commands.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}