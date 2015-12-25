using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiContrib.IoC.StructureMap;

namespace ApiApp
{
    /// <summary>
    /// The WebApi Configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registering the webapi configuration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.DependencyResolver = new StructureMapResolver(DiConfig.CreateContainer());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
