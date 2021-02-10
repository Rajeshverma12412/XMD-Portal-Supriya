using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace AmscoMncrData
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

    }

    public class PreflightRequestsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

              if (request.Headers.Contains("Origin") && request.Method.Equals("OPTIONS"))
              {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            // Define and add values to variables: origins, headers, methods (can be global)               
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Access-Control-Allow-Origin ,Access-Control-Allow-Headers, Authorization, Accept");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow‌​-Credentials", "true");
            response.Headers.Add("Access-Control-Max-Age", "1728000");
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
             }
            return base.SendAsync(request, cancellationToken);
        }
    }


}
