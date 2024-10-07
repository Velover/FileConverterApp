using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using ImageMagick;

namespace ConverterUI.Utils
{
	internal class FileIconExtractor
	{
		private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
		{
			using var memory = new MemoryStream();
			bitmap.Save(memory, ImageFormat.Png);
			memory.Position = 0;

			var bitmap_image = new BitmapImage();
			bitmap_image.BeginInit();
			bitmap_image.StreamSource = memory;
			bitmap_image.CacheOption = BitmapCacheOption.OnLoad;
			bitmap_image.EndInit();
			bitmap_image.Freeze();

			return bitmap_image;
		}

		public static BitmapImage GetFileIcon(string file_path)
		{
			var icon = Icon.ExtractAssociatedIcon(file_path);
			if (icon is null)
				return new BitmapImage();

			var bitmap = icon.ToBitmap();
			return BitmapToBitmapImage(bitmap);
		}

		public static BitmapImage? TryGetFileImagePreview(string file_path)
		{
			try
			{
				//var uri = new Uri(file_path);
				//var original_bitmap_image = new BitmapImage(uri);
				//float target_biggest_side = 60;
				//var max_side = Math.Max(original_bitmap_image.Width, original_bitmap_image.Height);
				//var scale = target_biggest_side / (float)max_side;

				//var bitmap_image = new BitmapImage();
				//bitmap_image.BeginInit();
				//bitmap_image.UriSource = uri;
				//bitmap_image.DecodePixelWidth = (int)(original_bitmap_image.Width * scale);
				//bitmap_image.DecodePixelHeight = (int)(original_bitmap_image.Height * scale);
				//bitmap_image.EndInit();
				//bitmap_image.Freeze();

				//return bitmap_image;

				using var image = new MagickImage(file_path);
				var geometry = new MagickGeometry(60, 60);
				image.Resize(geometry);

				using var memory_stream = new MemoryStream();
				image.Write(memory_stream);

				var bitmap_image = new BitmapImage();
				bitmap_image.BeginInit();
				bitmap_image.StreamSource = memory_stream;
				bitmap_image.EndInit();
				bitmap_image.Freeze();

				return bitmap_image;
			}
			catch { }
			return null;
		}
	}
}
