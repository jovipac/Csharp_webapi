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

            var cors = new System.Web.Http.Cors.EnableCorsAttribute(
                "http://192.168.0.37:8100,http://localhost:8100",
                "*",
                "GET, POST, PATCH, PUT, DELETE, OPTIONS");
            config.EnableCors(cors);

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
