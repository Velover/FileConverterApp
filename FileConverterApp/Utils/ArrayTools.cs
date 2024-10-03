namespace FileConverterCore.Utils
{
	internal class ArrayTools
	{
		public static bool TrySelectFirst<T>(T[] array, out T value)
		{
			if (array.Length == 0)
			{
				value = default(T)!;
				return false;
			}

			value = array[0];
			return true;
		}
	}
}
