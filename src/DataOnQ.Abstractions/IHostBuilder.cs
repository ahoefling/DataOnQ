using System;
using DataOnQ.Abstractions;

namespace DataOnQ.Core
{
	public interface IHostBuilder
	{
		void Start();
		event EventHandler<ServiceProviderEventArgs> ContainerInitialized;

		// We might want to moves this out to an extension method
		IHostBuilder AddStartup(IDataOnQStartup startup);
		IHostBuilder AddStartup<TStartup>() where TStartup : IDataOnQStartup;
		IHostBuilder BuildContainer(Func<IDependencyBuilder> dependencyBuilder);
	}

	public class ServiceProviderEventArgs : EventArgs
	{
		public IServiceProvider ServiceProvider { get; }

		public ServiceProviderEventArgs(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}
	}
}
