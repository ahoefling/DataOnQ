using DataOnQ.Abstractions;
using DataOnQ.Core;
using DataOnQ.Middleware;
using Xamarin.Essentials;

namespace DataOnQ.Plugins.Http.HttpOfflineAvailable
{
	public partial class HttpOfflineAvailableMiddleware : BinaryTreeMiddleware
	{
		public class NetworkStatusHandler : IServiceHandler
		{
			public IHandlerResponse Handle<TService>(IMessageProxy<TService> payload)
			{
				if (Connectivity.NetworkAccess == NetworkAccess.Internet)
				{
					if (payload.PreviousResponse == null)
						payload.PreviousResponse = new HandlerResponse(null, true);
					else
						payload.PreviousResponse = new HandlerResponse(payload.PreviousResponse.GetResult<object>(), true);

					return payload.PreviousResponse;
				}

				return new HandlerResponse(null, false);
			}
		}
	}
}
