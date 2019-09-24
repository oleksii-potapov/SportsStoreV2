using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace SportsStore.Infrastructure
{
    public class RouteConfig
    {
        public static void MapRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List" });

            routes.MapRoute(
                name: null,
                template: "Page{productPage:int}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });

            routes.MapRoute(
                name: null,
                template: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });

            routes.MapRoute(
                name: "default",
                template: "",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });

            routes.MapRoute(
                name: "",
                template: "{controller}/{action}/{id?}");
        }
    }
}