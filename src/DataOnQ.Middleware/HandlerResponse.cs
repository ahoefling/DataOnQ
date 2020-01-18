using DataOnQ.Abstractions;

namespace DataOnQ.Middleware
{
	public class HandlerResponse : IHandlerResponse
	{
		private object _result;
		public HandlerResponse(object result, bool isSuccess)
		{
			_result = result;
			IsSuccess = isSuccess;
		}

		public bool IsSuccess { get; private set; }
		public T GetResult<T>()
		{
			return (T)_result;
		}
	}
}
