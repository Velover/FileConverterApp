namespace FileConverterCore.Converters
{
	internal interface IConverter
	{
		public void ConvertFileToFormat(string file_path, string format);
	}
}
