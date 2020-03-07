using System;
using System.Linq.Expressions;
using DataOnQ.Abstractions;
using DataOnQ.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Core
{
	public class ServiceWrapper<TService>
	{
		protected IServiceHandler Middleware { get; }
		public ServiceWrapper()
		{
			Middleware = DataOnQ.Container.GetService<IServiceHandler>();
		}

		protected virtual T Proxy<T>(Expression<Func<TService, object>> action)
		{
			var command = new MessageProxy<TService>(action);
			var response = Middleware.Handle(command);
			return response.IsSuccess ? response.GetResult<T>() : default(T);
		}
	}
}
