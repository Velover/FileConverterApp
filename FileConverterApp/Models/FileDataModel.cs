using FileConverterCore;

namespace FileConverterApp.Models
{
	public class FileDataModel
	{
		public string RawName { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public string FileFormat { get; set; } = string.Empty;
		public string[] AvailableFormats { get; set; } = [];
		public string SelectedFormat { get; set; } = string.Empty;
		public EConvertionStatus ConvertionStatus { get; set; } = EConvertionStatus.None;
		public string? ConvertionMessage { get; set; } = null;
	}
}
