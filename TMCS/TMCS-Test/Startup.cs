using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Threading;
using System.Text;

namespace TMCS_Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddCors((options) =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                 {
                     builder.AllowAnyOrigin();
                 });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());

            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseMvc();
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    if (context.Request.Path == "/ws")
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var handler = new WebSocketHandler(webSocket);
                        await handler.WaitHandShake();
                        TMCSTest.HandlerList[handler.Uid] = handler;
                        await handler.StartReceiev();
                    }
                    else
                    {
                        await next();
                    }
                }

            });
        }
    }
}
