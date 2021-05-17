using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Soul.DynamicWebApi;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRemoteServices(this IServiceCollection services, Action<RemoteServiceOptions> configure = null)
        {
            var option = new RemoteServiceOptions();
            configure?.Invoke(option);
            services.Configure<MvcOptions>(o =>
            {
                o.Conventions.Add(new RemoteServiceApplicationModelConvention(option));
            });
            var partManager = services.Where(a => a.ServiceType == typeof(ApplicationPartManager))
                .FirstOrDefault();
            if (partManager == null)
            {
                throw new NullReferenceException("Please inject after the controllers");
            }
            (partManager.ImplementationInstance as ApplicationPartManager)
                .FeatureProviders.Add(new RemoteServiceControllerFeatureProvider());
            return services;
        }
    }
}
