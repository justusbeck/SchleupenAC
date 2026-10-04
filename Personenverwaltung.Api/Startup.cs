using System.Web.Http;
using Microsoft.Owin;
using Owin;


namespace Personenverwaltung.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
            app.UseWebApi(config);
        }
    }
}