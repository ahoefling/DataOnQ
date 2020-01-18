using System;
using System.Linq.Expressions;
using DataOnQ.Abstractions;

namespace DataOnQ.Middleware
{
	internal class ServiceWrapper<TService>
	{
		protected IServiceHandler Middleware { get; }
		public ServiceWrapper(IServiceHandler middleware)
		{
			Middleware = middleware;
		}

		protected virtual T Proxy<T>(Expression<Func<TService, object>> action)
		{
			var command = new MessageProxy<TService>(action);
			var response = Middleware.Handle(command);
			return response.IsSuccess ? response.GetResult<T>() : default(T);
		}
	}
}
