using System;

namespace DataOnQ.Middleware
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class MiddlewareAttribute : Attribute
	{
		public Type Middleware { get; set; }
		public MiddlewareAttribute() { }
		public MiddlewareAttribute(Type middleware)
		{
			Middleware = middleware;
		}
	}
}
