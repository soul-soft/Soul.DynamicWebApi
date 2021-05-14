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

namespace Soul.DynamicWebAPI
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
                    item.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(_options.ServiceNamePrefix + "[controller]"));
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
            foreach (var selectorModel in actionModel.Selectors.Where(a => a.ActionConstraints.Count == 0))
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
                var templateBulder = new StringBuilder();
                foreach (var parameterModel in actionModel.Parameters)
                {
                    if (string.Equals(parameterModel.Name, "ID", StringComparison.InvariantCultureIgnoreCase))
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromRouteAttribute() });
                        templateBulder.Append("{" + parameterModel.Name + "}");
                    }
                    else if (method == HttpMethods.Get || parameterModel.ParameterType.IsValueType || parameterModel.ParameterType == typeof(string))
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromQueryAttribute() });
                    }
                    else
                    {
                        parameterModel.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
                if (selectorModel.AttributeRouteModel == null)
                {
                    selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(templateBulder.ToString()));
                }
            }
        }

        private string ApplyControllerName(string name)
        {
            foreach (var item in _options.ServiceNameEndTrims)
            {
                var index = name.LastIndexOf(item);
                if (index > 0)
                {
                    name = name.Substring(0, name.Length - item.Length);
                }
            }
            return name;
        }
    }
}