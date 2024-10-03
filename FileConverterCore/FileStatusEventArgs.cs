namespace FileConverterCore
{
	public class FileStatusEventArgs : EventArgs
	{
		public string FilePath { get; set; } = "";
		public string Message { get; set; } = string.Empty;
		public EConvertionStatus ConvertionStatus { get; set; } = EConvertionStatus.Error;
	}
}
