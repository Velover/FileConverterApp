using System.Windows;
using FileConverterApp.Controllers;
using FileConverterApp.StateManagement;
using FileConverterApp.Views;

namespace FileConverterApp
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			AppController.can_convert.OnChanged += OnCanConvertChanged;
			AppController.can_convert.TriggerFor(OnCanConvertChanged);

			AppController.available_global_formats.OnChanged += OnAvailableGlobalFormatsChanged;
			AppController.available_global_formats.TriggerFor(OnAvailableGlobalFormatsChanged);

			AppController.selected_global_format.OnChanged += OnGlobalSelectedFormatChanged;
			AppController.selected_global_format.TriggerFor(OnGlobalSelectedFormatChanged);

			AppController.files_exist.OnChanged += OnFilesExistChanged;
			AppController.files_exist.TriggerFor(OnFilesExistChanged);

			var files_list_container = new FilesListContainerView();
			FilesListContainerHolder.Children.Add(files_list_container);
		}

		private void OnAvailableGlobalFormatsChanged(
			object sender,
			ValueChangedEventArgs<string[]> args
		)
		{
			GlobalFormatComboBox.Items.Clear();
			foreach (var available_format in args.Value)
			{
				GlobalFormatComboBox.Items.Add(available_format);
			}
		}

		private void OnGlobalSelectedFormatChanged(object sender, ValueChangedEventArgs<string> args)
		{
			GlobalFormatComboBox.SelectedIndex = GlobalFormatComboBox.Items.IndexOf(args.Value);
		}

		private void OnCanConvertChanged(object sender, ValueChangedEventArgs<bool> args)
		{
			ConvertFilesButton.IsEnabled = args.Value;
		}

		private void OnFilesExistChanged(object sender, ValueChangedEventArgs<bool> args)
		{
			ClearFilesButton.IsEnabled = args.Value;
		}

		private void OnClearFilesButton_Clicked(object sender, RoutedEventArgs e)
		{
			AppController.ClearFileDataModelList();
		}

		private void OnConvertFilesButton_Clicked(object sender, RoutedEventArgs e)
		{
			AppController.StartConvertion();
		}

		private void GlobalFormatComboBox_OnSelectionChanged(
			object sender,
			System.Windows.Controls.SelectionChangedEventArgs e
		)
		{
			AppController.SetSelectedGlobalFormat((string)GlobalFormatComboBox.SelectedItem ?? "");
		}
	}
}
