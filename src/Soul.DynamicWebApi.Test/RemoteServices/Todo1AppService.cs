using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Soul.DynamicWebApi;

namespace Soul.DynamicWebApi.Test.RemoteServices
{
    public class Todo1AppService : IRemoteService
    {
        readonly ILogger<Todo1AppService> _logger;
        public Todo1AppService(ILogger<Todo1AppService> logger)
        {
            _logger = logger;
        }
        public Task CreateMyJobAsync(Todo1Dto dto)
        {
            _logger.LogInformation("Createxxx");
            return Task.CompletedTask;
        }

        public Task UpdateAsync()
        {
            _logger.LogInformation("Updatexxx");
            return Task.CompletedTask;
        }
        public Task UpdateJobAsync(int id, Todo1Dto dto)
        {
            _logger.LogInformation("Updatexxx (id ,dto)");
            return Task.CompletedTask;
        }
        public Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deletexxx (id)");
            return Task.CompletedTask;
        }
        public Task DeleteJobAsync(int id)
        {
            _logger.LogInformation("Deletexxx (id)");
            return Task.CompletedTask;
        }
        public Task<Todo1Dto> GetAsync(int id)
        {
            _logger.LogInformation("Getxxx (id)");
            return Task.FromResult(new Todo1Dto());
        }
        public Task<List<Todo1Dto>> GetListAsync(Todo1Model dto)
        {
            _logger.LogInformation("Getxxx (dto)");
            return Task.FromResult(new List<Todo1Dto>());
        }
    }
    public class Todo1Dto
    {
        public string Time { get; set; }
        public string Address { get; set; }
    }
    public class Todo1Model
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
