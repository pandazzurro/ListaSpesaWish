using ListaSpesaWish.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ListaSpesaWish
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Servizi e configurazione dell'API Web
            //Database.SetInitializer<ListaSpesaContext>(null);

            // Route dell'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
