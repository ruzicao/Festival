using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Festival.Models;
using Festival.Repository;
using Festival.Repository.Interfaces;
using Festival.Resolver;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;

namespace Festival
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Event, EventDTO>();
                cfg.CreateMap<Event, EventDetailDTO>();
                // automatski će mapirati Author.Name u AuthorName
                //.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name)); // ako želimo eksplicitno zadati mapranje
            });

            config.EnableSystemDiagnosticsTracing();

            var cors = new EnableCorsAttribute("*", "*", "*");

            config.EnableCors(cors);
            // Unity
            var container = new UnityContainer();
            container.RegisterType<IPlaceRepository, PlaceRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEventRepository, EventRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


        }
    }
}
