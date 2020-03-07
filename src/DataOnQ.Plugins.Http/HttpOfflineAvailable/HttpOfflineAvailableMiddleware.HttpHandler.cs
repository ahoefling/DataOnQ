using System;
using DataOnQ.Abstractions;
using DataOnQ.Core;

namespace DataOnQ.Plugins.Http.HttpOfflineAvailable
{
	public partial class HttpOfflineAvailableMiddleware : BinaryTreeMiddleware
	{
		public class HttpHandler : ServiceHandler
		{
			public override IHandlerResponse Handle<TService>(IMessageProxy<TService> proxy)
			{
				if (proxy.PreviousResponse?.GetResult<object>() != null)
					throw new InvalidOperationException($"The {nameof(HttpHandler)} must be called first, another handler returned a response!");

				return base.Handle(proxy);
			}
		}
	}
}
