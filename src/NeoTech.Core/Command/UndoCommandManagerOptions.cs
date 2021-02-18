using System;

namespace NeoTech.Core.Command
{
	/// <summary>
	/// Holds options to configure the behavior of an <see cref="UndoCommandManager"/>.
	/// </summary>
	public sealed class UndoCommandManagerOptions
	{
		/// <summary>
		/// Gets or sets the number of commands an <see cref="UndoCommandManager"/> can manage.
		/// Defaults to <see cref="Int32.MaxValue"/>.
		/// </summary>
		public int CommandMemory { get; set; } = Int32.MaxValue;
	}
}
