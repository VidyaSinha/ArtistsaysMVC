using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Blog",
                url: "blog/{action}/{id}",
                defaults: new { controller = "Home", action = "BlogList", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Gallery",
                url: "gallery",
                defaults: new { controller = "Home", action = "GalleryView" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "SignIn", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "Review",
                 url: "Home/Review", // URL structure to access the Review action
                 defaults: new { controller = "Home", action = "Review" }
             );


            routes.MapRoute(
                name: "Suggestion",
                url: "suggestion",
                defaults: new { controller = "Home", action = "Suggestion" }
            );

            routes.MapRoute(
               name: "Transport",
               url: "Transport/{action}/{id}",
               defaults: new { controller = "Home", action = "Transport", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "SignIn",
                url: "SignIn",
                defaults: new { controller = "Home", action = "SignIn" }
             );

            routes.MapRoute(
                name: "SignUp",
                url: "SignUp",
                defaults: new { controller = "Home", action = "SignUp" }
             );
        }
    }
}
