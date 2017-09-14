using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using TMCS.Server.Models;

namespace TMCS.Server
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.AddDbContext<TMCSServerContext>(options =>
                    options.UseSqlite("Data Source=TMCS.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Trace);

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
            app.UseStaticFiles();

            app.UseMvc();
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        while (true)
                        {
                            WebSocketReceiveResult result;
                            do
                            {
                                var msg = new List<byte>();
                                do
                                {
                                    var buffer = new byte[4096];
                                    result = await webSocket.ReceiveAsync(
                                        new ArraySegment<byte>(buffer),
                                        CancellationToken.None);
                                    var segment = new ArraySegment<byte>(buffer, 0, result.Count);
                                    msg.AddRange(segment);
                                    //await webSocket.SendAsync(
                                    //    new ArraySegment<byte>(
                                    //        segment.Reverse().ToArray(), 0, result.Count),
                                    //    result.MessageType,
                                    //    result.EndOfMessage,
                                    //    CancellationToken.None);
                                } while (!result.EndOfMessage);
                                var msgString = Encoding.UTF8.GetString(msg.ToArray());
                                //
                            } while (!result.CloseStatus.HasValue);
                            await webSocket.CloseAsync(
                                result.CloseStatus.Value,
                                result.CloseStatusDescription,
                                CancellationToken.None);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
