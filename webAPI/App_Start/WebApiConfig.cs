using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using WebService.Controllers;

namespace WebService
{
    public static class WebServiceConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // CConfiguración de rutas y servicios de API
            EnableCrossSiteRequests(config);

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {   /*
            var coors = new System.Web.Http.Cors.EnableCorsAttribute(
                "http://192.168.0.37:8100,http://localhost:8100",
                "*",
                "GET, POST, PATCH, PUT, DELETE, OPTIONS");
            config.EnableCors(coors);
            */
            // Declare local variables to store the CORS settings loaded from the web.config
            String str_origins;
            String str_headers;
            String str_methods;

            // load the CORS settings from the webConfig file
            str_origins = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            str_headers = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            str_methods = ConfigurationManager.AppSettings["JWT_METHODS"];

            var cors = new System.Web.Http.Cors.EnableCorsAttribute(
                origins: str_origins,
                headers: str_headers,
                methods: str_methods);
            config.EnableCors(cors);
        }

    }
}
