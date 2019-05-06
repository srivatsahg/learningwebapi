﻿using Microsoft.Web.Http;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TheCodeCamp
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services
      AutofacConfig.Register();

            //API Versioning
            config.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 1);
                //so that user does not specify in the url
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
            });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                      new CamelCasePropertyNamesContractResolver();

      // Web API routes
      config.MapHttpAttributeRoutes();

      //config.Routes.MapHttpRoute(
      //    name: "DefaultApi",
      //    routeTemplate: "api/{controller}/{id}",
      //    defaults: new { id = RouteParameter.Optional }
      //);

    }
  }
}
