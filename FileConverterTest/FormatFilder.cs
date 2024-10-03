using ImageMagick;

namespace FileConverterTest
{
	internal class FormatFilder
	{
		public static void ListAvailableFormats()
		{
			using var image = new MagickImage(@"C:\Users\cobau\Desktop\Resources\Images\2x2Image.png");


			var formats = MagickNET.SupportedFormats.Where(format => format.SupportsReading && format.SupportsWriting).Select(format => format.Format);

			List<string> supported_formats = new();

			foreach (var format in formats)
			{
				try
				{
					image.Format = format;
					image.Write($"Images/Test.{format.ToString()}");
					supported_formats.Add(format.ToString());
				}
				catch { }
			}


			Console.WriteLine($"{{{string.Join(",", supported_formats.Select(format => $"\"{format}\""))}");
		}
	}
}
