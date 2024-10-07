using FileConverterApp.Models;
using FileConverterApp.StateManagement;
using FileConverterApp.Utils;
using FileConverterCore;
using System.IO;

namespace FileConverterApp.Controllers
{
	internal class AppController
	{
		public static Value<bool> is_converting = new(false);
		public static Value<bool> can_convert = new(false);
		public static Value<bool> files_exist = new(false);
		public static Value<List<FileDataModel>> file_data_models_list = new([]);

		public static Value<string> selected_global_format = new(string.Empty);
		public static Value<string[]> available_global_formats = new([]);

		public static Value<EFileType> global_file_type = new(EFileType.None);
		public static readonly string USE_GLOBAL_FORMAT = "*";

		private static List<ConvertionDataModel> convertion_queue = new();

		public delegate void OnFileDataModelStatusChangedEventHandler(FileDataModel file_data_model);
		public static event OnFileDataModelStatusChangedEventHandler OnFileDataModelStatusChanged;

		private static bool init_ = false;
		public static void LazyInit()
		{
			if (init_) return;
			init_ = true;

			FileConverter.OnFileStatusChanged += OnFileStatusChanged;
		}

		private static void OnFileStatusChanged(object sender, FileStatusEventArgs args)
		{
			var file_data_model = file_data_models_list.GetValue().Find(file_data_model => file_data_model.Path == args.FilePath);
			if (file_data_model is null) return;
			App.Current.Dispatcher.Invoke(() =>
			{
				ChangeFileDataModelStatus(file_data_model, args.ConvertionStatus, args.Message);
				ReevaluateCanConvert();
			});
		}

		public static void SetSelectedGlobalFormat(string format)
		{
			selected_global_format.SetValue(format);
		}

		private static void ChangeFileDataModelStatus(FileDataModel file_data_model, EConvertionStatus convertion_status, string? message)
		{
			file_data_model.ConvertionStatus = convertion_status;
			file_data_model.ConvertionMessage = message;

			OnFileDataModelStatusChanged?.Invoke(file_data_model);
		}

		private static void ClearFileDataModelConvertStatus(FileDataModel file_data_model)
		{
			file_data_model.ConvertionStatus = EConvertionStatus.None;
			file_data_model.ConvertionMessage = string.Empty;
			ReevaluateCanConvert();
		}

		private static void OnGlobalFileTypeChanged()
		{
			if (global_file_type.GetValue() == EFileType.None)
			{
				selected_global_format.SetValue(string.Empty);
				available_global_formats.SetValue([]);
				return;
			}

			available_global_formats.SetValue(FileConverter.GetSupportedFormatsForFileType(global_file_type.GetValue()));
			selected_global_format.SetValue(available_global_formats.GetValue().FirstOrDefault(string.Empty));
		}

		private static void ReevaluateCanConvert()
		{
			can_convert.SetValue(
				files_exist.GetValue() &&
				file_data_models_list.GetValue().Exists(file_data_model => file_data_model.ConvertionStatus == EConvertionStatus.None)
			);
		}

		private static void OnFileDataModelsListChanged()
		{
			var file_data_models = file_data_models_list.GetValue();
			if (file_data_models.Count > 0)
			{
				files_exist.SetValue(true);
				ReevaluateCanConvert();
				return;
			}
			files_exist.SetValue(false);
			ReevaluateCanConvert();

			global_file_type.SetValue(EFileType.None);
			OnGlobalFileTypeChanged();
		}

		public static void AddFiles(string[] files)
		{
			var files_data_models = file_data_models_list.GetValue();
			//removes duplicates from existing list and the current list
			files = files
				.ToHashSet()
				.ToArray()
				.Where(file_path => !files_data_models.Exists(file_data_model => file_data_model.Path == file_path))
				.ToArray();



			if (global_file_type.GetValue() == EFileType.None)
			{
				var file_type = FileConverter.GetFileTypeFromFiles(files);
				if (file_type == EFileType.None) return;
				global_file_type.SetValue(file_type);
				OnGlobalFileTypeChanged();
			}

			var filtered_files = FileConverter.FilterFilesByType(files, global_file_type.GetValue());
			var file_data_models = filtered_files.Select(file_path => ConvertFileToFileDataModel(file_path)).ToArray();

			var file_data_models_list_clone = file_data_models_list.GetValue().ToList();
			file_data_models_list_clone.AddRange(file_data_models);
			file_data_models_list.SetValue(file_data_models_list_clone);
			OnFileDataModelsListChanged();
		}

		private static bool CanRemove(FileDataModel file_data_model)
		{
			return file_data_model.ConvertionStatus != EConvertionStatus.Converting;
		}

		public static void RemoveFileDataModel(FileDataModel file_data_model)
		{
			if (!CanRemove(file_data_model)) return;
			var file_data_models_list_clone = file_data_models_list.GetValue().ToList();
			file_data_models_list_clone.Remove(file_data_model);
			file_data_models_list.SetValue(file_data_models_list_clone);
			//TODO better removing from the convertion queue
			ListTools.TryRemoveWithPredicate(convertion_queue, (match => match.FilePath == file_data_model.Path));
			OnFileDataModelsListChanged();
		}

		public static void ClearFileDataModelList()
		{
			var file_data_models_clone = file_data_models_list.GetValue();
			foreach (var file_data_model in file_data_models_clone)
			{
				if (!CanRemove(file_data_model)) continue;
				file_data_models_clone.Remove(file_data_model);
				ListTools.TryRemoveWithPredicate(convertion_queue, (match => match.FilePath == file_data_model.Path));
			}

			file_data_models_list.SetValue(file_data_models_clone);
			OnFileDataModelsListChanged();
		}

		public static void SelectFormatForFileDataModel(FileDataModel file_data_model, string format)
		{
			file_data_model.SelectedFormat = format;
		}

		public static FileDataModel ConvertFileToFileDataModel(string file_path)
		{
			var raw_name = Path.GetFileNameWithoutExtension(file_path) ?? "";
			var format = Path.GetExtension(file_path) ?? "";
			var supported_write_formats = FileConverter.GetSupportedFormatsForFileType(global_file_type.GetValue());
			var available_formats = (new string[] { USE_GLOBAL_FORMAT }).Concat(supported_write_formats).ToArray();

			return new FileDataModel()
			{
				AvailableFormats = available_formats,
				RawName = raw_name,
				Path = file_path,
				FileFormat = format.Substring(1),
				ConvertionStatus = EConvertionStatus.None,
				SelectedFormat = USE_GLOBAL_FORMAT,
			};
		}

		public static void StartConvertion()
		{

			//Could be reworked :D
			if (!files_exist.GetValue()) return;
			if (is_converting.GetValue()) return;
			is_converting.SetValue(true);
			foreach (var file_data_model in file_data_models_list.GetValue())
			{
				var selected_format = file_data_model.SelectedFormat == USE_GLOBAL_FORMAT ? selected_global_format.GetValue() : file_data_model.SelectedFormat;
				convertion_queue.Add(ConvertFileDataModelToConvertionDataModel(file_data_model, selected_format, global_file_type.GetValue()));
				ChangeFileDataModelStatus(file_data_model, EConvertionStatus.Starting, null);
			}

			Task.Run(() =>
			{
				while (convertion_queue.Count > 0)
				{
					var convertion_data_model = convertion_queue[0];
					convertion_queue.RemoveAt(0);
					FileConverter.ConvertFileToFormat(convertion_data_model.FilePath, convertion_data_model.FileType, convertion_data_model.ToFormat);
				}
				App.Current.Dispatcher.Invoke(() =>
				{
					is_converting.SetValue(false);
				});
			});
		}

		private static ConvertionDataModel ConvertFileDataModelToConvertionDataModel(FileDataModel file_data_model, string format, EFileType file_type)
		{
			return new ConvertionDataModel()
			{
				FilePath = file_data_model.Path,
				ToFormat = format,
				FileType = file_type
			};
		}
	}
}
