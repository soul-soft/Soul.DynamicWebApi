using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soul.DynamicWebAPI.Test.RemoteServices
{
    public class Todo2AppService:IRemoteService
    {

        [HttpPost("do1")]
        public Task Createxxx1()
        {
            return Task.CompletedTask;
        }
        [HttpPost("do2")]
        public Task Createxxx2()
        {
            return Task.CompletedTask;
        }
    }
}
