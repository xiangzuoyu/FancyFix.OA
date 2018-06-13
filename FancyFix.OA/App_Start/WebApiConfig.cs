using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;

namespace FancyFix.OA
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
    
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional
                }
            );
            ConfigureWebApi(config);
        }
        public static void ConfigureWebApi(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            
        }

        
    }
    
}
