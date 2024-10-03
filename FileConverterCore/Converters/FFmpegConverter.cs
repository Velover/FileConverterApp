using FFMpegCore;
using FileConverterCore.Utils;

namespace FileConverterCore.Converters
{
	internal class FFmpegConverter : IConverter
	{
		public static readonly string[] SupportedReadAudioFormats = { "mp3", "acc", "wav", "flac", "ogg", "wma", "alac", "ac3", "opus", "pcm", "adpcm" };
		public static readonly string[] SupportedWriteAudioFormats = { "mp3", "acc", "wav", "flac", "ogg", "wma", "alac", "ac3", "opus", "pcm", "adpcm" };

		public static readonly string[] SupportedReadVideoFormats = { "mp4", "avi", "mkv", "mov", "flv", "mwv", "webm", "mpeg", "3gp", "ogv", "mts", "m2ts", "ts" };
		public static readonly string[] SupportedWriteVideoFormats = { "mp4", "avi", "mkv", "mov", "flv", "mwv", "webm", "mpeg", "3gp", "ogv", "mts", "m2ts", "ts" };

		private static bool is_bin_setup_ = false;
		private static void LazyBinSetup()
		{
			if (is_bin_setup_) return;
			is_bin_setup_ = true;
			GlobalFFOptions.Configure(options => options.BinaryFolder = @"ffmpeg_7.1\bin");
		}
		public void ConvertFileToFormat(string file_path, string format)
		{
			LazyBinSetup();
			var new_path = PathCreatorHelper.ChangeFormatAndGenerateNewPath(file_path, format);
			FFMpegArguments.FromFileInput(file_path).OutputToFile(new_path).ProcessSynchronously(true);
		}
	}
}
