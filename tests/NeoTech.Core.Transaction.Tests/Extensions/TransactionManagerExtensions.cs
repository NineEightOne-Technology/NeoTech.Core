using System;
using System.Reflection;
using Microsoft;
using NeoTech.Core.Transactions;

namespace NeoTech.Core.Transaction.Tests.Extensions
{
	public static class TransactionManagerExtensions
	{
		public static void SimulateFinalTransactionPhase(this ITransactionManager transactionManager)
		{
			Requires.NotNull(transactionManager, nameof(transactionManager));

			var property = transactionManager
				.GetType()
				.GetProperty("InFinalPhase", BindingFlags.Instance | BindingFlags.NonPublic);

			if (property == null)
				throw new InvalidOperationException();

			property
				.SetValue(transactionManager, true);
		}
	}
}