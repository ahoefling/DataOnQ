using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DataOnQ.Abstractions;
using DataOnQ.Core;
using DataOnQ.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace DataOnQ.Samples.Xamarin.Forms
{
	public class Startup : IDataOnQStartup
	{
		void IDataOnQStartup.ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IToDoService, ToDoService>();
			services.AddTransient<IToDoService, ToDoHttpService>();
			services.AddTransient<IToDoService, ToDoReadDatabaseService>();
			services.AddTransient<IToDoService, ToDoWriteDatabaseService>();
		}
	}

	[Middleware(null)]
	public class ToDoService : ServiceWrapper<IToDoService>, IToDoService
	{
		public Task<IEnumerable<ToDoModel>> GetItems()
		{
			return Proxy<Task<IEnumerable<ToDoModel>>>(x => x.GetItems());
		}
	}

	public interface IToDoService
	{
		Task<IEnumerable<ToDoModel>> GetItems();
	}

	public class ToDoModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Details { get; set; }
	}

	public class ToDoHttpService : IToDoService
	{
		public async Task<IEnumerable<ToDoModel>> GetItems()
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetAsync("https://www.google.com");
				if (!result.IsSuccessStatusCode)
					return null;

				return new[]
				{
					new ToDoModel(),
					new ToDoModel()
				};
			}
		}
	}

	public class ToDoReadDatabaseService : IToDoService
	{
		public Task<IEnumerable<ToDoModel>> GetItems()
		{
			throw new System.NotImplementedException();
		}
	}

	public class ToDoWriteDatabaseService : IToDoService
	{
		public Task<IEnumerable<ToDoModel>> GetItems()
		{
			throw new System.NotImplementedException();
		}
	}
}
