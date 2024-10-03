using FileConverterApp.Controllers;
using System.Windows;

namespace FileConverterApp
{
	public partial class App : Application
	{
		private void OnAppStartUp(object sender, StartupEventArgs args)
		{
			string[] test_args = [@"C:\Users\cobau\Desktop\Resources\Images\2x2Image.png"];

			AppController.LazyInit();
			AppController.AddFiles(args.Args.Length == 0 ? test_args : args.Args);
			var main_window = new MainWindow();
			main_window.Show();
		}
	}
}
