using System.Windows.Controls;
using FileConverterApp.Controllers;
using FileConverterApp.Models;
using FileConverterApp.StateManagement;
using FileConverterApp.Views.FilesListContainer;

namespace FileConverterApp.Views
{
	public partial class FilesListContainerView : UserControl
	{
		Dictionary<FileDataModel, FileContainer> file_containers_dictionary_ = new();

		public FilesListContainerView()
		{
			InitializeComponent();

			AppController.file_data_models_list.OnChanged += OnFileDataModelsListChnaged;
			AppController.file_data_models_list.TriggerFor(OnFileDataModelsListChnaged);

			AppController.OnFileDataModelStatusChanged += OnFileDataModelStatusChanged;
		}

		private void OnFileDataModelStatusChanged(FileDataModel file_data_model)
		{
			var success = file_containers_dictionary_.TryGetValue(
				file_data_model,
				out var file_container
			);
			if (!success)
				return;
			file_container!.OnFileDataModelStatusUpdated();
		}

		private FileContainer CreateFileContainerForFileDataModel(FileDataModel file_data_model)
		{
			var file_container = new FileContainer(file_data_model);
			file_containers_dictionary_.Add(file_data_model, file_container);
			return file_container;
		}

		private void OnFileDataModelsListChnaged(
			object sender,
			ValueChangedEventArgs<List<FileDataModel>> args
		)
		{
			FilesHolder.Children.Clear();
			foreach (var file_data_model in args.Value)
			{
				FileContainer file_container;
				var success = file_containers_dictionary_.TryGetValue(file_data_model, out file_container!);
				if (!success)
				{
					file_container = CreateFileContainerForFileDataModel(file_data_model);
				}
				FilesHolder.Children.Add(file_container);
			}
		}
	}
}
