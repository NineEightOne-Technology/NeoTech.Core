using System;

namespace NeoTech.Core.Command
{
	public sealed class UndoCommandManagerOptions
	{
		public int CommandMemory { get; set; } = Int32.MaxValue;
	}
}
