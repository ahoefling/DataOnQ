using System;
using System.Collections;
using System.Collections.Generic;
using DataOnQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Core
{
	// usage generic:
	// DataOnQ.HostBuilder<Startup>()
	//		  .BuildContainer(c => new DependencyBuilder())
	//		  .Start()
	// usage generic:
	// DataOnQ.HostBuilder<Startup>()
	//		  .BuildContainer(c => new XamarinDependencyBuilder())
	//		  .Start()
	// usage generic:
	// DataOnQ.HostBuilder<Startup>()
	//		  .BuildContainer(c => new PrismDependencyBuilder())
	//		  .Start()

	public interface IRegistrationHandler
	{
		IRegistrationHandlerFrom From<T>();
	}

	public interface IRegistrationHandlerFrom
	{
		void To<T>();
	}

	public static class DataOnQ
	{
		public static IRegistrationHandler RegisterHandler<THandler>()
		{
			return null;
		}









		public static IHostBuilder HostBuilder<TStartup>()
			where TStartup : IDataOnQStartup
		{
			return new HostBuilder(Activator.CreateInstance<TStartup>());
		}

		public static IHostBuilder HostBuilder(IDataOnQStartup startup)
		{
			return new HostBuilder(startup);
		}

		internal static IServiceProvider Container { get; set; }
	}

	public class HostBuilder : IHostBuilder
	{
		IList<IDataOnQStartup> _startupCollection;
		IServiceCollection _services;
		public HostBuilder(IDataOnQStartup startup)
		{
			_startupCollection = new List<IDataOnQStartup>();
			_startupCollection.Add(startup);
		}

		public event EventHandler<ServiceProviderEventArgs> ContainerInitialized;

		public IHostBuilder AddStartup(IDataOnQStartup startup)
		{
			if (_startupCollection == null)
				throw new NullReferenceException("Startup Collection can't be null, be sure to use the Host Builder during Application Startup Routine");

			_startupCollection.Add(startup);
			return this;
		}

		public IHostBuilder AddStartup<TStartup>()
			where TStartup : IDataOnQStartup
		{
			return AddStartup(Activator.CreateInstance<TStartup>());
		}

		public IHostBuilder BuildContainer(Func<IDependencyBuilder> dependencyBuilder)
		{
			_services = dependencyBuilder()
				.BuildServiceCollection();
			return this;
		}

		public void Start()
		{
			if (_services == null)
				BuildContainer(() => new DefaultDependencyBuilder());

			foreach (var startup in _startupCollection)
				startup.ConfigureServices(_services);

			// this needs to be stored as an internal container somewhere
			DataOnQ.Container = _services.BuildServiceProvider(); // is this still needed if we have an event to get the container?

			if (ContainerInitialized != null)
				ContainerInitialized.Invoke(this, new ServiceProviderEventArgs(DataOnQ.Container));
		}
	}

	public class DefaultDependencyBuilder : IDependencyBuilder
	{
		IServiceCollection _collection;
		public IServiceProvider BuildProvider()
		{
			if (_collection == null)
				return null;

			return _collection.BuildServiceProvider();
		}

		public IServiceCollection BuildServiceCollection()
		{
			_collection = new ServiceCollection();
			return _collection;
		}
	}
}
