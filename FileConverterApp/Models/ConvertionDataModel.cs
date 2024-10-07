using FileConverterCore;

namespace FileConverterApp.Models
{
	internal class ConvertionDataModel
	{
		public string FilePath { get; set; }
		public string ToFormat { get; set; }
		public EFileType FileType { get; set; }
	}
}
