using System;
using Xamarin.Forms.Internals;

namespace DataOnQ.Core
{
	public static class HostBuilder
	{
		public static IHostBuilder UseXamarinDependencyResolver(this IHostBuilder builder)
		{
			builder.ContainerInitialized += OnContainerInitialized;
			IServiceProvider container = default(IServiceProvider);

			DependencyResolver.ResolveUsing(t => container?.GetService(t));
			return builder;

			void OnContainerInitialized(object sender, ServiceProviderEventArgs eventArgs)
			{
				container = eventArgs.ServiceProvider;
				builder.ContainerInitialized -= OnContainerInitialized;
			}
		}
	}
}
