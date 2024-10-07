using System.Windows.Controls;
using ConverterUI.Utils;
using FileConverterApp.Controllers;
using FileConverterApp.Models;
using FileConverterCore;

namespace FileConverterApp.Views.FilesListContainer
{
	public partial class FileContainer : UserControl
	{
		private FileDataModel file_data_model_;

		public FileContainer(FileDataModel file_data_model)
		{
			file_data_model_ = file_data_model;
			InitializeComponent();
			FileNameText.Text = $"Name: {file_data_model_.RawName}";
			FileFormatText.Text = $"Format: .{file_data_model_.FileFormat}";

			OnFileDataModelStatusUpdated();

			FillFormatsComboBox();
			FileImagePreview.Source = FileIconExtractor.GetFileIcon(file_data_model.Path);
		}

		private void FillFormatsComboBox()
		{
			//justin case
			FormatSelectionCombobox.Items.Clear();

			foreach (var available_format in file_data_model_.AvailableFormats)
			{
				FormatSelectionCombobox.Items.Add(available_format);
			}
			FormatSelectionCombobox.SelectedIndex = FormatSelectionCombobox.Items.IndexOf(
				file_data_model_.SelectedFormat
			);
		}

		public void OnFileDataModelStatusUpdated()
		{
			FormatSelectionCombobox.IsEnabled = (
				(file_data_model_.ConvertionStatus == EConvertionStatus.None)
				|| (file_data_model_.ConvertionStatus == EConvertionStatus.Done)
			);
			DeleteFileButton.IsEnabled =
				file_data_model_.ConvertionStatus != EConvertionStatus.Converting;

			StatusText.Text = $"Status: {file_data_model_.ConvertionStatus.ToString()}";
			if (string.IsNullOrEmpty(file_data_model_.ConvertionMessage))
				return;
			StatusText.Text += $"\nMessage: {file_data_model_.ConvertionMessage}";
		}

		private void OnFormatSelectionCombobox_SelectionChanged(
			object sender,
			SelectionChangedEventArgs e
		)
		{
			var format = (FormatSelectionCombobox.SelectedItem as string) ?? string.Empty;
			AppController.SelectFormatForFileDataModel(file_data_model_, format);
		}

		private void OnDeleteFileButton_Clicked(object sender, System.Windows.RoutedEventArgs e)
		{
			AppController.RemoveFileDataModel(file_data_model_);
		}
	}
}
