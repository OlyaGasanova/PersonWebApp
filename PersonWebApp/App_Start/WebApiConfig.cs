using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PersonWebApp {

    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "PersonApi",
                routeTemplate: "api/{controller}/{args}/{name}",
                defaults: new { args = RouteParameter.Optional, name = RouteParameter.Optional }
            );

            

        }

    }

}
