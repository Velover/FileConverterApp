using FileConverterCore.Utils;
using ImageMagick;

namespace FileConverterCore.Converters
{
	internal class ImageConverter : IConverter
	{
		public static readonly string[] SupportedReadFormats = { "a", "aai", "ai", "art", "avif", "avs", "b", "bayer", "bayera", "bgr", "bgra", "bgro", "bmp", "bmp2", "bmp3", "cal", "cals", "cin", "clipboard", "cmyk", "cmyka", "cur", "dcx", "dds", "dpx", "dxt1", "dxt5", "epdf", "epi", "eps", "epsf", "epsi", "ept", "ept2", "ept3", "exr", "farbfeld", "fax", "ff", "fits", "fl32", "fts", "ftxt", "g", "g3", "g4", "gif", "gif87", "gray", "graya", "group4", "hdr", "hrz", "icb", "ico", "icon", "ipl", "j2c", "j2k", "jng", "jp2", "jpc", "jpe", "jpeg", "jpg", "jpm", "jps", "jxl", "map", "mat", "miff", "mng", "mono", "mpc", "msvg", "mtv", "null", "o", "otb", "pal", "palm", "pam", "pbm", "pcd", "pcds", "pcl", "pct", "pcx", "pdb", "pdf", "pdfa", "pfm", "pgm", "pgx", "phm", "picon", "pict", "pjpeg", "png", "png00", "png24", "png32", "png48", "png64", "png8", "pnm", "pocketmod", "ppm", "ps", "psb", "psd", "ptif", "qoi", "r", "ras", "rgb", "rgba", "rgbo", "rgf", "rsvg", "sgi", "six", "sixel", "strimg", "sun", "svg", "svgz", "tga", "tiff", "tiff64", "txt", "uyvy", "vda", "vicar", "vid", "viff", "vips", "vst", "wbmp", "webp", "wpg", "xbm", "xpm", "xv", "ycbcr", "ycbcra", "yuv", "dib", "tif" };
		public static readonly string[] SupportedWriteFormats = { "avif", "bmp", "dds", "exr", "gif", "ico", "j2k", "jpeg", "jpg", "jxl", "pdf", "png", "psd", "svg", "tga", "tiff", "webp", "xbm", "xpm", "tif" };

		private static MagickFormat GetMagickFormatForImageFormat(string format)
		{
			var formats = MagickNET.SupportedFormats.Select(format => format.Format);
			foreach (var magick_format in formats)
			{
				if (magick_format.ToString().ToLower() == format) return magick_format;
			}
			throw new Exception($"Format .{format} is not supported");
		}

		public void ConvertFileToFormat(string file_path, string format)
		{
			var magick_format = GetMagickFormatForImageFormat(format);
			var new_path = PathCreatorHelper.ChangeFormatAndGenerateNewPath(file_path, format);
			using var image = new MagickImage(file_path);
			image.Write(new_path, magick_format);
		}
	}
}
