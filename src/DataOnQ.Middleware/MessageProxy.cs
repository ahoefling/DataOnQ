using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DataOnQ.Abstractions;

namespace DataOnQ.Middleware
{
	public class MessageProxy<TService> : IMessageProxy<TService>, IMessageProxyCommand<TService>
	{
		Expression<Func<TService, object>> _command;
		string _name;

		public MessageProxy(Expression<Func<TService, object>> command)
		{
			_command = command;
		}

		public IHandlerResponse PreviousResponse { get; set; }

		Expression<Func<TService, object>> IMessageProxyCommand<TService>.Command => _command;

		IHandlerResponse IMessageProxy<TService>.Execute(TService service)
		{
			var action = _command.Compile();
			return new HandlerResponse(action(service), true);
		}
	}
}
