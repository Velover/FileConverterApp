namespace FileConverterApp.Utils
{
	internal class ListTools
	{
		public static void TryRemoveValue<T>(List<T> list, T value)
		{
			var index = list.IndexOf(value);
			if (index == -1) return;
			list.RemoveAt(index);
		}
		public static void TryRemoveAt<T>(List<T> list, int index)
		{
			if (index < 0) return;
			if (index >= list.Count) return;
			list.RemoveAt(index);
		}

		public static void TryRemoveWithPredicate<T>(List<T> list, Predicate<T> predicate)
		{
			var index = list.FindIndex(predicate);
			TryRemoveAt(list, index);
		}
	}
}
