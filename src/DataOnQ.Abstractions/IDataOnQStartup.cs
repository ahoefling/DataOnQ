using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Abstractions
{
	public interface IDataOnQStartup
	{
		void ConfigureServices(IServiceCollection services);
	}
}
