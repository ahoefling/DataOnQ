using DataOnQ.Core;
using DataOnQ.Plugins.Http.HttpOfflineAvailable;
using DataOnQ.Samples.Xamarin.Forms.Services;
using DataOnQ.Samples.Xamarin.Forms.Views;
using Xamarin.Forms;

namespace DataOnQ.Samples.Xamarin.Forms
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			// REVIEW - the initial startup code needs some tweaking
			DataOnQ.Core.DataOnQ
				.HostBuilder<Startup>()
				.BuildContainer(() => new DefaultDependencyBuilder())
				.UseXamarinDependencyResolver()
				.Start();

			RegisterServices();

			DependencyService.Register<MockDataStore>();
			MainPage = new MainPage();
		}

		private void RegisterServices()
		{
			DataOnQ.Core.DataOnQ
				.RegisterHandler<HttpOfflineAvailableMiddleware.ReadLocalHandler>()
				.From<IToDoService>()
				.To<ToDoReadDatabaseService>();

			DataOnQ.Core.DataOnQ
				.RegisterHandler<HttpOfflineAvailableMiddleware.WriteLocalHandler>()
				.From<IToDoService>()
				.To<ToDoWriteDatabaseService>();

			DataOnQ.Core.DataOnQ
				.RegisterHandler<HttpOfflineAvailableMiddleware.HttpHandler>()
				.From<IToDoService>()
				.To<ToDoHttpService>();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
