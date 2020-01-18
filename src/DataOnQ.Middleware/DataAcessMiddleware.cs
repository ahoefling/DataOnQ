using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataOnQ.Abstractions;

namespace DataOnQ.Middleware
{
	public class DataAccessMiddleware : IServiceHandler, IServiceBuilder
	{
		public IList<IServiceHandler> Handlers = new List<IServiceHandler>();

		public IHandlerResponse Handle<TService>(IMessageProxy<TService> payload)
		{
			try
			{
				var overrideMiddleware = FindOverrideMiddleware(payload);
				if (overrideMiddleware != null)
				{
					return overrideMiddleware.Handle(payload);
				}

				foreach (var handler in Handlers)
				{
					var currentResponse = handler.Handle(payload);
					payload.PreviousResponse = currentResponse;
				}
			}
#if DEBUG
			catch (NullReferenceException ex)
			{
				throw new InvalidOperationException("The last executed Service Handler was not configured correctly, see inner exception for more details", ex);
			}
			catch (ResolutionFailedException ex)
			{
				throw new InvalidOperationException($"Unable to resolve {ex.TypeRequested} with a name of {ex.NameRequested}. This typically happens when a change has been made to the middleware manager. Check the Container to verify that all steps of the Middleware have been properly resolved. See inner exception for more details", ex);
			}
#endif
			catch (FaultException ex)
			{
				var reason = ex.Reason.ToString();
				if (reason.ToLower().Contains("not authorized"))
					throw new UnauthorizedAccessException("Device is not authorized with FileOnQ!", ex);
				else
					throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return payload.PreviousResponse;
		}

		private IServiceHandler FindOverrideMiddleware<TService>(IMessageProxy<TService> payload)
		{
			var serviceType = typeof(TService);

			if (payload is IMessageProxyCommand<TService> proxyCommand)
			{
				string methodName;
				Type[] parameters;
				switch (proxyCommand.Command.Body)
				{
					case MethodCallExpression m:
						methodName = m.Method.Name;
						parameters = m.Method.GetParameters()
							?.Select(x => x.ParameterType)
							?.ToArray();
						break;
					case UnaryExpression u when u.Operand is MethodCallExpression m:
						methodName = m.Method.Name;
						parameters = m.Method.GetParameters()
							?.Select(x => x.ParameterType)
							?.ToArray();
						break;
					default:
						throw new NotImplementedException("Unable to find Method name on given Expression, solution not implemented!");
				}

				var methodInfo = parameters != null ?
					serviceType.GetMethod(methodName, parameters) :
					serviceType.GetMethod(methodName);

				if (methodInfo != null)
				{
					var methodAttributes = (MiddlewareAttribute[])methodInfo.GetCustomAttributes(typeof(MiddlewareAttribute), true);
					if (methodAttributes?.Length > 0)
						return (IServiceHandler)Activator.CreateInstance(methodAttributes[0].Middleware);
				}
			}


			var attributes = (MiddlewareAttribute[])serviceType.GetCustomAttributes(typeof(MiddlewareAttribute), true);
			if (attributes?.Length > 0)
				return (IServiceHandler)Activator.CreateInstance(attributes[0].Middleware);

			return null;
		}

		public void Register<T>() where T : IServiceHandler
		{
			Handlers.Add(DataModule.Container.Resolve<T>());
		}
	}
}
