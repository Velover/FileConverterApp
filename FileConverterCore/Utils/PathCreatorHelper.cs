namespace FileConverterCore.Utils
{
	internal class PathCreatorHelper
	{
		public static string ChangeFormatAndGenerateNewPath(string file_path, string format)
		{
			var file_raw_name = Path.GetFileNameWithoutExtension(file_path);
			var directory = Path.GetDirectoryName(file_path);

			var new_name = $"{file_raw_name}.{format}";
			var attempt = 1;
			while (File.Exists($@"{directory}\{new_name}"))
			{
				new_name = $"{file_raw_name} ({attempt++}).{format}";
			}

			var new_path = $@"{directory}\{new_name}";
			return new_path;
		}
	}
}
