using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AmscoMncrData
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            //enable CORS
          //  EnableCorsAttribute cors = new EnableCorsAttribute("http://192.168.1.6:7500/api/AmscoMcnr", "*", "*");
            EnableCorsAttribute cors = new EnableCorsAttribute("http://192.168.1.6:5500,http://192.168.1.6:80", "*", "*") { SupportsCredentials = true };
            config.EnableCors(cors);

            config.MessageHandlers.Add(new PreflightRequestsHandler());


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

          
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
  
        }

    }
}
