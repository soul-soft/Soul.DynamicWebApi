using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soul.DynamicWebAPI
{
    public class RemoteServiceOptions
    {
        public string ControllerNamePrefix { get; set; } = string.Empty;
        public string ServiceNamePrefix { get; set; } = "api";
        public ICollection<string> ServiceNameEndTrims { get; } = new List<string>()
        {
            "AppService"
        };
    }
}
