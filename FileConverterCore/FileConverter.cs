using FileConverterCore.Converters;

namespace FileConverterCore
{
	public class FileConverter
	{
		public static readonly string[] SupportedReadImageFormats = ImageConverter.SupportedReadFormats;
		public static readonly string[] SupportedWriteImageFormats = ImageConverter.SupportedWriteFormats;

		public static readonly string[] SupportedReadAudioFormats = FFmpegConverter.SupportedReadAudioFormats;
		public static readonly string[] SupportedWriteAudioFormats = FFmpegConverter.SupportedWriteAudioFormats;

		public static readonly string[] SupportedReadVideoFormats = FFmpegConverter.SupportedReadVideoFormats;
		public static readonly string[] SupportedWriteVideoFormats = FFmpegConverter.SupportedWriteVideoFormats;

		public delegate void OnFileStatusChangedEventHandler(object sender, FileStatusEventArgs args);
		public static event OnFileStatusChangedEventHandler OnFileStatusChanged;

		private static IConverter image_converter_ = new ImageConverter();
		private static IConverter ffmpeg_converter_ = new FFmpegConverter();

		static bool IsInSupportedFormatsArray(string format, string[] formats_array)
		{
			var lower_format = format.ToLower();
			foreach (var existing_format in formats_array)
			{
				if (existing_format == lower_format) return true;
			}
			return false;
		}
		public static EFileType GetFileType(string file_path)
		{
			if (!File.Exists(file_path)) return EFileType.None;
			var format = Path.GetExtension(file_path).Substring(1);
			Console.WriteLine(format);

			if (IsInSupportedFormatsArray(format, SupportedReadImageFormats)) return EFileType.Image;
			else if (IsInSupportedFormatsArray(format, SupportedReadAudioFormats)) return EFileType.Audio;
			else if (IsInSupportedFormatsArray(format, SupportedReadVideoFormats)) return EFileType.Video;

			return EFileType.None;
		}

		public static EFileType GetFileTypeFromFiles(string[] files)
		{
			foreach (var file_path in files)
			{
				var file_type = GetFileType(file_path);

				if (file_type != EFileType.None) return file_type;
			}
			return EFileType.None;
		}

		public static string[] FilterFilesByType(string[] files, EFileType file_type)
		{
			return files.Where(file_path => GetFileType(file_path) == file_type).ToArray();
		}

		public static string[] GetSupportedFormatsForFileType(EFileType file_type)
		{
			var formats_list = file_type switch
			{
				EFileType.Audio => SupportedWriteAudioFormats,
				EFileType.Video => SupportedWriteVideoFormats,
				EFileType.Image => SupportedWriteImageFormats,
				_ => []
			};

			return formats_list.ToArray();
		}

		private static bool CanUseFormatForFileType(EFileType file_type, string format)
		{
			var formats_list = GetSupportedFormatsForFileType(file_type);
			if (IsInSupportedFormatsArray(format, formats_list)) return true;
			return false;
		}

		private static void ReportFileStatus(FileStatusEventArgs args)
		{
			if (OnFileStatusChanged is not null) OnFileStatusChanged(args.ConvertionStatus, args);
		}

		private static IConverter GetConverterForFileType(EFileType file_type)
		{
			if (file_type == EFileType.Image)
			{
				return image_converter_;
			}
			else if (file_type == EFileType.Video)
			{
				return ffmpeg_converter_;
			}
			else if (file_type == EFileType.Audio)
			{
				return ffmpeg_converter_;
			}

			throw new Exception($"Converter is not implemented for type {file_type.ToString()}");
		}

		public static void ConvertFileToFormat(string file_path, EFileType file_type, string format)
		{
			if (!CanUseFormatForFileType(file_type, format))
			{
				ReportFileStatus(new FileStatusEventArgs
				{
					FilePath = file_path,
					ConvertionStatus = EConvertionStatus.Error,
					Message = $"Format {format} is not supported with file type {file_type.ToString()}"
				});
				return;
			}

			ReportFileStatus(new FileStatusEventArgs
			{
				FilePath = file_path,
				ConvertionStatus = EConvertionStatus.Starting,
			});


			try
			{
				var converter = GetConverterForFileType(file_type);
				ReportFileStatus(new FileStatusEventArgs
				{
					FilePath = file_path,
					ConvertionStatus = EConvertionStatus.Converting,
				});
				converter.ConvertFileToFormat(file_path, format);

				ReportFileStatus(new FileStatusEventArgs
				{
					FilePath = file_path,
					ConvertionStatus = EConvertionStatus.Done,
				});
			}
			catch (Exception exception)
			{
				ReportFileStatus(new FileStatusEventArgs
				{
					FilePath = file_path,
					ConvertionStatus = EConvertionStatus.Error,
					Message = exception.Message
				});
			}
		}
	}
}
