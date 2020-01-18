using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
	public static class DataOnQ
	{
		public static IHostBuilder HostBuilder<TStartup>()
			where TStartup : IDataOnQStartup
		{
			return new HostBuilder(Activator.CreateInstance<TStartup>());
		}

		public static IHostBuilder HostBuilder(IDataOnQStartup startup)
		{
			return new HostBuilder(startup);
		}

		public static dynamic Platform { get; set; }
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
		}
	}
	

	public interface IHostBuilder
	{
		void Start();
	}

	public interface IDependencyBuilder
	{
		IServiceCollection BuildServiceCollection();
	}

	public class DefaultDependencyBuilder : IDependencyBuilder
	{
		public IServiceCollection BuildServiceCollection()
		{
			// TODO - this should use the microsoft.extensions IServiceProvider
			throw new NotImplementedException();
		}
	}
}
