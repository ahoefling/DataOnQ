using System;
using DataOnQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Core
{
	public abstract class BinaryTreeMiddleware : IServiceHandler
	{
		protected BinaryTree<IServiceHandler> HandlerTree { get; set; }

		IServiceProvider _container;
		public BinaryTreeMiddleware(IServiceProvider container)
		{
			_container = container;
		}

		protected virtual IServiceHandler ResolveHandler<T>()
			where T : IServiceHandler
		{
			return _container.GetService<T>();
		}

		protected virtual IServiceHandler ResolveHandler(Type handlerType)
		{
			return (IServiceHandler)_container.GetService(handlerType);
		}

		public virtual IHandlerResponse Handle<TService>(IMessageProxy<TService> payload)
		{
			return HandleTree(HandlerTree);

			IHandlerResponse HandleTree(BinaryTree<IServiceHandler> node)
			{
				var result = node.Value.Handle(payload);
				payload.PreviousResponse = result;
				if (result.IsSuccess)
					return node.Left != null ? HandleTree(node.Left) : result;
				else
					return node.Right != null ? HandleTree(node.Right) : result;
			}
		}
	}
}
