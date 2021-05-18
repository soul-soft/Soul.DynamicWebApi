using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soul.DynamicWebApi
{
    internal class RemoteServiceApplicationModelConvention : IApplicationModelConvention
    {
        readonly RemoteServiceOptions _options;

        public RemoteServiceApplicationModelConvention(RemoteServiceOptions options)
        {
            _options = options;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controllerModel in application.Controllers)
            {
                ApplyControllerModel(controllerModel);
                foreach (var actionModel in controllerModel.Actions)
                {
                    ApplyActionModel(actionModel);
                }
            }
        }

        private void ApplyControllerModel(ControllerModel controllerModel)
        {
            if (typeof(IRemoteService).IsAssignableFrom(controllerModel.ControllerType))
            {
                if (controllerModel.ApiExplorer.IsVisible == null)
                {
                    controllerModel.ApiExplorer.IsVisible = true;
                }
                controllerModel.ControllerName = ApplyControllerName(controllerModel.ControllerName);
                foreach (var item in controllerModel.Selectors)
                {
                    if (!controllerModel.Attributes.Any(a => a.GetType() == typeof(ApiControllerAttribute)))
                    {
                        item.EndpointMetadata.Add(new ApiControllerAttribute());
                    }
                    if (!controllerModel.Attributes.Any(a => a.GetType() == typeof(ControllerAttribute)))
                    {
                        item.EndpointMetadata.Add(new ControllerAttribute());
                    }
                    if (!controllerModel.Attributes.Any(a => a.GetType() == typeof(ControllerAttribute)))
                    {
                        item.EndpointMetadata.Add(new RouteAttribute("[controller]"));
                    }
                    if (item.AttributeRouteModel == null)
                    {
                        item.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute("[controller]"));
                    }
                    if (!string.IsNullOrEmpty(_options.ServiceNamePrefix))
                    {
                        var prefixRoute = new AttributeRouteModel(new RouteAttribute(_options.ServiceNamePrefix));
                        item.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(prefixRoute, item.AttributeRouteModel);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(_options.ControllerNamePrefix))
            {
                foreach (var item in controllerModel.Selectors)
                {
                    if (item.AttributeRouteModel == null)
                    {
                        item.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(_options.ControllerNamePrefix));
                    }
                    else
                    {
                        item.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(new AttributeRouteModel(new RouteAttribute(_options.ControllerNamePrefix)), item.AttributeRouteModel);
                    }
                }
            }
        }

        private void ApplyActionModel(ActionModel actionModel)
        {
            if (actionModel.ApiExplorer.IsVisible == null)
            {
                actionModel.ApiExplorer.IsVisible = true;
            }
            foreach (var selectorModel in actionModel.Selectors)
            {
                //如果不存在httpMethod
                if (selectorModel.ActionConstraints.Count == 0)
                {
                    var method = HttpMethods.Post;
                    if (actionModel.ActionName.StartsWith("Get", StringComparison.CurrentCultureIgnoreCase))
                    {
                        method = HttpMethods.Get;
                        selectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                    }
                    else if (actionModel.ActionName.StartsWith("Update", StringComparison.CurrentCultureIgnoreCase))
                    {
                        method = HttpMethods.Put;
                        selectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                    }
                    else if (actionModel.ActionName.StartsWith("Delete", StringComparison.CurrentCultureIgnoreCase))
                    {
                        method = HttpMethods.Delete;
                        selectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                    }
                    else
                    {
                        selectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                    }
                    selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { method }));
                    if (selectorModel.AttributeRouteModel == null)
                    {
                        selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(ApplyActionName(actionModel.ActionName)));
                    }
                    var templateBulder = new StringBuilder();
                    foreach (var parameterModel in actionModel.Parameters)
                    {
                        if (string.Equals(parameterModel.Name, "ID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            templateBulder.Append("{" + parameterModel.Name + "}");
                        }
                    }
                    if (templateBulder.Length > 0)
                    {
                        var routeModel = new AttributeRouteModel(new RouteAttribute(templateBulder.ToString()));
                        selectorModel.AttributeRouteModel = AttributeRouteModel
                                .CombineAttributeRouteModel(selectorModel.AttributeRouteModel, routeModel);
                    }
                }
                foreach (var parameterModel in actionModel.Parameters)
                {
                    //如果存在设置模型绑定
                    if (parameterModel.BindingInfo != null)
                    {
                        continue;
                    }
                    if (string.Equals(parameterModel.Name, "ID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromRouteAttribute() });
                    }
                    else if (selectorModel.EndpointMetadata.Any(a => a.GetType() == typeof(HttpGetAttribute)) || parameterModel.ParameterType.IsValueType || parameterModel.ParameterType == typeof(string))
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromQueryAttribute() });
                    }
                    else
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }

        private string ApplyControllerName(string name)
        {
            foreach (var item in _options.ServiceNameEndTrims)
            {
                name = name.RightTrim(item);
            }
            return name;
        }

        private string ApplyActionName(string name)
        {
            foreach (var item in new string[] { "GET", "CREATE", "UPDATE", "DELETE" })
            {
                if (name.StartsWith(item, StringComparison.CurrentCultureIgnoreCase))
                {
                    name = name.LeftTrim(item);
                    break;
                }
            }
            name = name.RightTrim("Async");
            name = name.RightTrim("List");
            var sb = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && i != 0)
                {
                    sb.Append('/');
                }
                sb.Append(name[i]);
            }
            return sb.ToString();
        }
    }
}