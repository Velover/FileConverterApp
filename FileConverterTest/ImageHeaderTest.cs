﻿namespace FileConverterTest
{
	internal class ImageHeaderTest
	{
		public static void Init()
		{
			var headers = new Dictionary<string, byte[][]>
{
		{ "Avif", new byte[][] {
			new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66 },
			new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x69, 0x66, 0x31 },
			new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66 }
		} },
		{ "Bmp", new byte[][] {
			new byte[] { 0x42, 0x4D }
		} },
		{ "Dds", new byte[][] {
			new byte[] { 0x44, 0x44, 0x53, 0x20 }
		} },
		{ "Exr", new byte[][] {
			new byte[] { 0x76, 0x2F, 0x31, 0x01 }
		} },
		{ "Gif", new byte[][] {
			new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 },
			new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }
		} },
		{ "Ico", new byte[][] {
			new byte[] { 0x00, 0x00, 0x01, 0x00 }
		} },
		{ "J2k", new byte[][] {
			new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20 },
			new byte[] { 0xFF, 0x4F, 0xFF, 0x51 }
		} },
		{ "Jpeg", new byte[][] {
			new byte[] { 0xFF, 0xD8, 0xFF }
		} },
		{ "Jpg", new byte[][] {
			new byte[] { 0xFF, 0xD8, 0xFF }
		} },
		{ "Jxl", new byte[][] {
			new byte[] { 0xFF, 0x0A }
		} },
		{ "Pdf", new byte[][] {
			new byte[] { 0x25, 0x50, 0x44, 0x46 }
		} },
		{ "Png", new byte[][] {
			new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
		} },
		{ "Psd", new byte[][] {
			new byte[] { 0x38, 0x42, 0x50, 0x53 }
		} },
		{ "Svg", new byte[][] {
			new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C }
		} },
		{ "Tga", new byte[][] {
			new byte[] { 0x00, 0x00, 0x02, 0x00 },
			new byte[] { 0x00, 0x00, 0x03, 0x00 },
			new byte[] { 0x00, 0x00, 0x10, 0x00 }
		} },
		{ "Tiff", new byte[][] {
			new byte[] { 0x49, 0x49, 0x2A, 0x00 },
			new byte[] { 0x4D, 0x4D, 0x00, 0x2A }
		} },
		{ "WebP", new byte[][] {
			new byte[] { 0x52, 0x49, 0x46, 0x46 }
		} },
		{ "Xbm", new byte[][] {
			new byte[] { 0x23, 0x20, 0x58, 0x42, 0x4D },
			new byte[] { 0x23, 0x64, 0x65, 0x66, 0x69, 0x6E, 0x65, 0x20 }
		} },
		{ "Xpm", new byte[][] {
			new byte[] { 0x2F, 0x2A, 0x20, 0x58, 0x50, 0x4D }
		} },
		{ "Tif", new byte[][] {
			new byte[] { 0x49, 0x49, 0x2A, 0x00 },
			new byte[] { 0x4D, 0x4D, 0x00, 0x2A }
		} }
};


			foreach (var entry in headers)
			{
				var success = false;
				foreach (var header in entry.Value)
				{
					using var file_reader = new FileStream($"Images/Test.{entry.Key}", FileMode.Open);
					var found_header = true;
					foreach (var header_byte in header)
					{
						var read_byte = file_reader.ReadByte();
						found_header = found_header && (header_byte == read_byte);
					}
					if (found_header)
					{
						success = true;
						break;
					}
				}
				if (!success)
				{
					Console.WriteLine($"{entry.Key} didnt pass");
					continue;
				};
				Console.WriteLine($"{entry.Key} passed");
				//Console.WriteLine($"Key: {entry.Key} Value: {entry.Value}");
			}
		}
	}
}
