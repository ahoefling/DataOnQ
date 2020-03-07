using System;
using DataOnQ.Abstractions;
using DataOnQ.Middleware;

namespace DataOnQ.Core
{
	public abstract class ServiceHandler : IServiceHandler
	{
		public IHandlerResponse Handle(IMessageProxy payload)
		{
			throw new NotImplementedException();
		}

		public virtual IHandlerResponse Handle<TService>(IMessageProxy<TService> proxy)
		{
			try
			{ 
				// I'm not 100% sure if this GetService call is necessary since it will just be retrieving itself
				var service = GetService<TService>();
				if (service is IAttachProxy hasAttachments)
					hasAttachments.Attach(proxy);

			
				return proxy.Execute(service);
			}
			catch (Exception ex)
			{
#if DEBUG
				System.Diagnostics.Debug.WriteLine($"{ex.GetType().Name}");
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
#endif
				return new HandlerResponse(null, false);
			}
		}

		protected virtual TService GetService<TService>()
		{
			return (TService)DataOnQ.Container.GetService(this.GetType());
		}
	}
}
