using System;

namespace NeoTech.Core.Command
{
	public sealed class UndoCommandManagerOptions
	{
		public bool RetainCommands { get; set; } = false;

		public int CommandMemory { get; set; } = Int32.MaxValue;
	}
}
