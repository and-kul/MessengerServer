using System;
using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Nancy.TinyIoc;

namespace MessengerServer
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        //protected override DiagnosticsConfiguration DiagnosticsConfiguration
        //    => new DiagnosticsConfiguration {Password = @"Innopolis"};


        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += ctx =>
            {
                if (!ctx.Request.Headers.Accept.Any())
                    ctx.Request.Headers.Accept = new[] {Tuple.Create("application/json", 1m)};

                if (string.IsNullOrEmpty(ctx.Request.Headers.ContentType))
                    ctx.Request.Headers.ContentType = "application/json";

                return null;
            };


            //StaticConfiguration.EnableRequestTracing = true;
            //StaticConfiguration.DisableErrorTraces = false;
            
            var configuration =
                new StatelessAuthenticationConfiguration(ctx =>
                {
                    var jwtToken = ctx.Request.Headers.Authorization;

                    try
                    {
                        using (var db = new MessengerDbContext())
                        {
                            var payload = Jose.JWT.Decode<JwtToken>(jwtToken, db.Secrets.Find(1).Key);

                            var tokenExpires = DateTime.FromBinary(payload.exp);

                            if (DateTime.UtcNow < tokenExpires)
                            {
                                return new Identity(payload.id);
                            }

                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                });

            StatelessAuthentication.Enable(pipelines, configuration);
        }
    }
}