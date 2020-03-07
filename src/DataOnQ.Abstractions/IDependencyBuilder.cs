using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Core
{
	public interface IDependencyBuilder
	{
		IServiceCollection BuildServiceCollection();
	}
}
