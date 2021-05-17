using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soul.DynamicWebApi.Test.RemoteServices
{
    [Route("Todo22")]
    public class Todo2AppService : IRemoteService
    {

        readonly ILogger<Todo1AppService> _logger;
        public Todo2AppService(ILogger<Todo1AppService> logger)
        {
            _logger = logger;
        }
        [HttpPost("Create")]
        public Task CreateAsync([FromForm]Todo1Dto dto)
        {
            _logger.LogInformation("private Createxxx");
            return Task.CompletedTask;
        }
        private Task CreateJobAsync()
        {
            _logger.LogInformation("private Createxxx");
            return Task.CompletedTask;
        }
        public Task CreateMyJobAsync()
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
}
