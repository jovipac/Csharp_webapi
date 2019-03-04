using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebService.Controllers;

namespace WebService
{
    public static class WebServiceConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // CConfiguración de rutas y servicios de API

            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
