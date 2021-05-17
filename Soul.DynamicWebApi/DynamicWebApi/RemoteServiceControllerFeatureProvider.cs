using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Soul.DynamicWebApi
{
    internal class RemoteServiceControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (typeInfo.IsInterface)
            {
                return false;
            }
            if (typeInfo.IsAbstract)
            {
                return false;
            }
            var type = typeInfo.AsType();
            if (typeof(IRemoteService).IsAssignableFrom(type) && type.IsClass)
            {
                return true;
            }
            return base.IsController(typeInfo);
        }
    }
}
