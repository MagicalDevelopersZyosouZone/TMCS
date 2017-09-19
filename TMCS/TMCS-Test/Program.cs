using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings;
using System.Net;

namespace TMCS_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running!!!");
            var data = Convert.ToBase64String(TMCSTest.RSAEncrypt(Encoding.UTF8.GetBytes("Test"), TMCSTest.PUBLIC_KEY));
            var txt = Encoding.UTF8.GetString(TMCSTest.RSADecrypt(Convert.FromBase64String(data), TMCSTest.PRIVATE_KEY));
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 57320);
                })
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5732);
                })
                .Build();
    }
}
