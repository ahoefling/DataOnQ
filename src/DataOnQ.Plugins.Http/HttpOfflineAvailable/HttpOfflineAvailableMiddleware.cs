using DataOnQ.Abstractions;
using DataOnQ.Core;

namespace DataOnQ.Plugins.Http.HttpOfflineAvailable
{
	public partial class HttpOfflineAvailableMiddleware : BinaryTreeMiddleware
	{
		public HttpOfflineAvailableMiddleware() : base(null)
		{
			var queryLocal = new BinaryTree<IServiceHandler>
			{
				Value = ResolveHandler<ReadLocalHandler>()
			};

			HandlerTree = new BinaryTree<IServiceHandler>
			{
				Value = ResolveHandler<NetworkStatusHandler>(),
				Left = new BinaryTree<IServiceHandler>()
				{
					Value = ResolveHandler<HttpHandler>(),
					Left = new BinaryTree<IServiceHandler>
					{
						Value = ResolveHandler<WriteLocalHandler>()
					},
					Right = queryLocal
				},
				Right = queryLocal
			};
		}
	}
}
