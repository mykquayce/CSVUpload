namespace System.Collections.Generic;

public static class EnumerableExtensions
{
	public static async IAsyncEnumerable<T> GetManyAsync<T>(this IEnumerable<IAsyncEnumerable<T>> valueses)
	{
		foreach (var values in valueses)
		{
			await foreach (var value in values)
			{
				yield return value;
			}
		}
	}
}
