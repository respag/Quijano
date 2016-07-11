using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NLog;
using System.Web.Http.Filters;

namespace AttachmentPage
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Quite los comentarios de la siguiente línea de código para habilitar la compatibilidad de consultas para las acciones con un tipo de valor devuelto IQueryable o IQueryable<T>.
            // Para evitar el procesamiento de consultas inesperadas o malintencionadas, use la configuración de validación en QueryableAttribute para validar las consultas entrantes.
            // Para obtener más información, visite http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Para deshabilitar el seguimiento en la aplicación, incluya un comentario o quite la siguiente línea de código
            // Para obtener más información, consulte: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

            // Utilice minúsculas para los datos JSON.
            config.Filters.Add(new UnhandledExceptionFilter());
        }
    }

    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            Logger logger = LogManager.GetLogger("WebApiConfig");

            logger.Fatal("[WebApiApplication / Application_Error] " + context.Exception.ToString());

        }
    }
}
